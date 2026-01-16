using Auth0.OidcClient;
using ClientApp.Models;
using ClientApp.Utils;
using Duende.IdentityModel.OidcClient;
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

        public LoginService(
            Auth0Client client, 
            IHttpClientFactory httpClientFactory, 
            UserInfoService userInfoService, 
            AuthTokenStorage authTokenStorage,
            AuthTokenService authTokenService )
        {
            _authClient = client;
            _httpClientFactory = httpClientFactory;
            _userInfoService = userInfoService;
            _authTokenStorage = authTokenStorage;
            _authTokenService = authTokenService;
        }

        /// <summary>
        /// Build the uri for logging in
        /// </summary>
        /// <param name="authId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private static string BuildUri(string? authId, string? email) => string.Format("/api/User/get/{0}/{1}", authId, email);

        /// <summary>
        /// Verify with the database that the user exists
        /// </summary>
        /// <remarks>
        /// TODO: Move this to the callback for auth0
        /// </remarks>
        /// <param name="auth0LoginResult"></param>
        /// <returns></returns>
        private async Task<User?> VerifyAuth0LoginAsync(LoginResult auth0LoginResult)
        {
            await _authTokenStorage.UpdateTokensAsync(auth0LoginResult.AccessToken, auth0LoginResult.RefreshToken);
            var userFromClaims = GetUserFromClaims(auth0LoginResult.User.Claims);
            if (userFromClaims != null)
            {
                return userFromClaims;
            }

            // create a user from the database - one doesn't exist yet

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
                var token = await _authTokenService.RefreshTokenAsync();
                if ( token != null )
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken( token );

                    if (jwtToken.ValidTo > DateTime.UtcNow)
                    {
                        var claims = jwtToken.Claims;
                        var userFromClaims = GetUserFromClaims(claims);
                        if (userFromClaims != null && !userFromClaims.Name.IsNullOrEmpty())
                        {
                            _userInfoService.SetUserInfo(userFromClaims);
                            return true;
                        }
                    }
                }

                var loginResult = await _authClient.LoginAsync(extraParameters: new Dictionary<string, string>
                    {
                        {
                            "audience", "https://gift-application-service-garfg.ondigitalocean.app/"}
                     });
                if (loginResult.IsError)
                {
                    // auth0 doesn't throw exceptions, throw it for them
                    throw new Exception(loginResult.Error);
                }

                var user = await VerifyAuth0LoginAsync(loginResult);


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
