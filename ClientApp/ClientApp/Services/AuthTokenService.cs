using Auth0.OidcClient;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace ClientApp.Services
{
    public class AuthTokenService
    {
        private DateTime _tokenExpiry = DateTime.MinValue;

        private readonly Auth0Client _auth0Client;
        private readonly AuthTokenStorage _authTokenStorage;
        private readonly ILogger<AuthTokenService> _logger;

        public AuthTokenService(Auth0Client auth0Client, AuthTokenStorage authTokenStorage, ILogger<AuthTokenService> logger )
        {
            _auth0Client = auth0Client;
            _authTokenStorage = authTokenStorage;
            _logger = logger;
        }

        public async Task<string?> RefreshTokenAsync()
        {
            var ( authToken, expiry ) = await GetTokenFromStorageAsync();
            _tokenExpiry = expiry;
            if (DateTime.UtcNow >= _tokenExpiry)
            {
                _logger.LogInformation("Token expired, refreshing token via server.");
                var refreshToken = await _authTokenStorage.GetRefreshTokenAsync();
                var result = await _auth0Client.RefreshTokenAsync(refreshToken, extraParameters: new Dictionary<string, string>
                {
                        {
                            "audience", "https://gift-application-service-garfg.ondigitalocean.app/"
                        }
                });
                
                if (!result.IsError)
                {
                    _logger.LogInformation("Updating token with new response");
                    await _authTokenStorage.UpdateTokensAsync(result.AccessToken, result.RefreshToken);
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(result.AccessToken);
                    _tokenExpiry = jwtToken.ValidTo;
                    return result.AccessToken;
                }
                else
                {
                    _logger.LogError("Authentication responded with {error} - {description}", result.Error, result.ErrorDescription);
                    await _authTokenStorage.UpdateTokensAsync(null, null);
                    return null;
                }
            }
            return authToken;
        }

        private async Task<(string?, DateTime)> GetTokenFromStorageAsync()
        {
            var accessToken = await _authTokenStorage.GetAuthTokenAsync();
            if (accessToken != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(accessToken);
                return (accessToken, jwtToken.ValidTo);
            }
            return (null, DateTime.MinValue);
        }
    }
}
