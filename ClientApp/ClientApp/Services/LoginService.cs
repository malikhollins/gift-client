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

        public LoginService(
            Auth0Client client,
            IHttpClientFactory httpClientFactory,
            UserInfoService userInfoService,
            AuthTokenStorage authTokenStorage,
            AuthTokenService authTokenService,
            ILogger<LoginService> logger)
        {
            _authClient = client;
            _httpClientFactory = httpClientFactory;
            _userInfoService = userInfoService;
            _authTokenStorage = authTokenStorage;
            _authTokenService = authTokenService;
            _logger = logger;
        }

        /// <summary>
        /// Build the uri for logging in
        /// </summary>
        /// <param name="authId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private static string BuildUri(string? authId, string? email) => string.Format("/api/User/get/{0}/{1}", authId, email);

        /// <summary>
        /// Create a user with the database
        /// </summary>
        /// <remarks>
        /// TODO: Move this to the callback for auth0
        /// </remarks>
        /// <param name="auth0LoginResult"></param>
        /// <returns></returns>
        private async Task<User?> CreateUserAsync(LoginResult auth0LoginResult)
        {

            try
            {
                // gather inputs for uri parameters
                var authId = auth0LoginResult.User.Claims.FirstOrDefault(claim => claim.Type == "sub");
                var email = auth0LoginResult.User.Claims.FirstOrDefault(claim => claim.Type == "name");
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

        public async Task<bool> LoginAsync()
        {
            try
            {
                var token = await _authTokenStorage.GetValidAuthTokenAsync();
                if (token != null)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var claims = jwtToken.Claims;
                    var userFromClaims = GetUserFromClaims(claims);
                    if (userFromClaims != null && !userFromClaims.Name.IsNullOrEmpty())
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

                var user  = GetUserFromClaims(loginResult.User.Claims);
                if (user != null)
                {
                    _userInfoService.SetUserInfo(user);
                    return true;
                }

                user = await CreateUserAsync(loginResult);
#if DEBUG
                _logger.LogInformation("Creating user finished - it took {time} seconds", stopwatch.Elapsed.TotalSeconds);
                stopwatch.Stop();
#endif
                _userInfoService.SetUserInfo(user);
                return user != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        private User? GetUserFromClaims(IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(claim => claim.Type == "https://gift-all.com/user_email");
            var userId = claims.FirstOrDefault(claim => claim.Type == "https://gift-all.com/user_id");
            var name = claims.FirstOrDefault(claim => claim.Type == "https://gift-all.com/user_name");

            if (userId != null)
            {
                return new User
                {
                    Email = email?.Value,
                    Id = int.Parse(userId.Value),
                    Name = name?.Value
                };
            }

            return null;
        }
    }
}
