using Auth0.OidcClient;
using System.IdentityModel.Tokens.Jwt;

namespace ClientApp.Services
{
    public class AuthTokenService
    {
        private DateTime _tokenExpiry = DateTime.MinValue;

        private readonly Auth0Client _auth0Client;
        private readonly AuthTokenStorage _authTokenStorage;

        public AuthTokenService(Auth0Client auth0Client, AuthTokenStorage authTokenStorage)
        {
            _auth0Client = auth0Client;
            _authTokenStorage = authTokenStorage;
        }

        public async Task<string?> RefreshTokenAsync()
        {
            if (DateTime.UtcNow >= _tokenExpiry)
            {
                var refreshToken = await _authTokenStorage.GetRefreshTokenAsync();

                var result = await _auth0Client.RefreshTokenAsync(refreshToken, extraParameters: new Dictionary<string, string>
                    {
                        {
                            "audience", "https://gift-application-service-garfg.ondigitalocean.app/"}
                     });
                                if (!result.IsError)
                {
                    await _authTokenStorage.UpdateTokensAsync(result.AccessToken, result.RefreshToken);
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(result.AccessToken);
                    _tokenExpiry = jwtToken.ValidTo;
                    return result.AccessToken;
                }
            }

            return await _authTokenStorage.GetAuthTokenAsync();
        }
    }
}
