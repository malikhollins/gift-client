using Auth0.OidcClient;
using ClientApp.Models;
using ClientApp.Utils;
using Duende.IdentityModel.OidcClient;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientApp.Services
{
    public class LoginService
    {
        private readonly Auth0Client _authClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserInfoService _userInfoService;
        private readonly AuthTokenStorage _authTokenStorage;
        private readonly AuthTokenService _authTokenService;
        private readonly ILogger<LoginService> _logger;
        private readonly AuthLoginCoordinator _loginCoordinator;

        public LoginService(
            Auth0Client client,
            IHttpClientFactory httpClientFactory,
            UserInfoService userInfoService,
            AuthTokenStorage authTokenStorage,
            AuthTokenService authTokenService,
            ILogger<LoginService> logger,
            AuthLoginCoordinator loginCoordinator)
        {
            _authClient = client;
            _httpClientFactory = httpClientFactory;
            _userInfoService = userInfoService;
            _authTokenStorage = authTokenStorage;
            _authTokenService = authTokenService;
            _logger = logger;
            _loginCoordinator = loginCoordinator;
        }

        /// <summary>
        /// Build the uri for logging in
        /// </summary>
        /// <param name="authId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private static string BuildUri(string? authId, string? email) => string.Format("/api/User/get/{0}/{1}", authId, email ?? "null");

        /// <summary>
        /// Create a user with the database
        /// </summary>
        /// <remarks>
        /// TODO: Move this to the callback for auth0
        /// </remarks>
        /// <param name="auth0LoginResult"></param>
        /// <returns></returns>
        public async Task<User?> GetUserAsync(IEnumerable<Claim> auth0LoginResult)
        {
            try
            {
                // gather inputs for uri parameters
                var authId = auth0LoginResult.FirstOrDefault(claim => claim.Type == "sub");
                var email = auth0LoginResult.FirstOrDefault(claim => claim.Type == "https://gift-all.com/user_email" && !string.IsNullOrWhiteSpace(claim.Value))
                    ?? auth0LoginResult.FirstOrDefault(claim => claim.Type == "email" && !string.IsNullOrWhiteSpace(claim.Value))
                    ?? auth0LoginResult.FirstOrDefault(claim => claim.Type == ClaimTypes.Email && !string.IsNullOrWhiteSpace(claim.Value));
                var uri = BuildUri(authId?.Value, email?.Value);

                HttpClient httpClient = _httpClientFactory.CreateClient("base-url");
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                return await response.DeserializeAsync<User>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> CanLoginAutomaticallyAsync()
        {
            if (!_loginCoordinator.TryEnter())
            {
                _logger.LogInformation("Login already in progress, skipping duplicate call.");
                return false;
            }

            try
            {
                var token = await _authTokenStorage.GetValidAuthTokenAsync();
                if (token != null)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var userFromClaims = GetUserFromClaims(jwtToken.Claims);
                    if (userFromClaims != null && !string.IsNullOrWhiteSpace(userFromClaims.Name))
                    {
                        return true;
                    }
                }

                return false;
            }
            finally
            {
                _loginCoordinator.Exit();
            }
        }

        public async Task<bool> LoginAsync()
        {
            // Prevent overlapping login flows without using a static flag.
            if (!_loginCoordinator.TryEnter())
            {
                _logger.LogInformation("Login already in progress, skipping duplicate call.");
                return false;
            }

            try
            {
                var token = await _authTokenStorage.GetValidAuthTokenAsync();
                if (token != null)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var tokenClaims = jwtToken.Claims;
                    var userFromClaims = GetUserFromClaims(tokenClaims);
                    if (userFromClaims != null && !string.IsNullOrWhiteSpace(userFromClaims.Name))
                    {
                        _userInfoService.SetUserInfo(userFromClaims);
                        return true;
                    }

                    var persistedUser = await GetUserAsync(tokenClaims);
                    if (persistedUser != null)
                    {
                        _userInfoService.SetUserInfo(persistedUser);
                        return true;
                    }

                    if (userFromClaims != null)
                    {
                        _userInfoService.SetUserInfo(userFromClaims);
                        return true;
                    }
                }
#if DEBUG
                var stopwatch = Stopwatch.StartNew();
#endif

                var loginResult = await _authClient.LoginAsync(extraParameters: new Dictionary<string, string>
                    {
                        {
                            "audience", "https://gift-application-service-garfg.ondigitalocean.app/"
                        }
                    });

                if (loginResult.IsError)
                {
                    // auth0 doesn't throw exceptions, throw it for them
                    throw new Exception(loginResult.Error, new Exception(loginResult.ErrorDescription));
                }

                await _authTokenStorage.UpdateTokensAsync(loginResult.AccessToken, loginResult.RefreshToken);


#if DEBUG
                _logger.LogInformation("Auth0 login finished - it took {time} seconds", stopwatch.Elapsed.TotalSeconds);
                stopwatch.Restart();
#endif

                var claims = loginResult.User.Claims;
                var user = GetUserFromClaims(claims);
                if (user != null && !string.IsNullOrWhiteSpace(user.Name))
                {
                    _userInfoService.SetUserInfo(user);
                    return true;
                }

                var userFromApi = await GetUserAsync(claims);
#if DEBUG
                _logger.LogInformation("Creating user finished - it took {time} seconds", stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
#endif
                user = userFromApi ?? user;
                _userInfoService.SetUserInfo(user);
                return user != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            finally
            {
                _loginCoordinator.Exit();
            }
        }

        private User? GetUserFromClaims(IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(claim => claim.Type == "https://gift-all.com/user_email" && !string.IsNullOrWhiteSpace(claim.Value));
            var userId = claims.FirstOrDefault(claim => claim.Type == "https://gift-all.com/user_id");
            var name = claims.FirstOrDefault(claim => claim.Type == "https://gift-all.com/user_name" && !string.IsNullOrWhiteSpace(claim.Value))
                ?? claims.FirstOrDefault(claim => claim.Type == "name" && !string.IsNullOrWhiteSpace(claim.Value))
                ?? claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name && !string.IsNullOrWhiteSpace(claim.Value))
                ?? claims.FirstOrDefault(claim => claim.Type == "nickname" && !string.IsNullOrWhiteSpace(claim.Value));

            if (userId != null && int.TryParse(userId.Value, out var parsedUserId))
            {
                return new User
                {
                    Email = email?.Value
                        ?? claims.FirstOrDefault(claim => claim.Type == "email" && !string.IsNullOrWhiteSpace(claim.Value))?.Value
                        ?? claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email && !string.IsNullOrWhiteSpace(claim.Value))?.Value,
                    Id = parsedUserId,
                    Name = name?.Value
                };
            }

            return null;
        }
    }
}
