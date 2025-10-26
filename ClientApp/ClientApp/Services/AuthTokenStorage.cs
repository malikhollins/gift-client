namespace ClientApp.Services
{
    public class AuthTokenStorage
    {
        public async Task UpdateTokensAsync(string? authToken, string? refreshToken)
        {
            if (authToken != null)
            {
                await SecureStorage.SetAsync("auth_token", authToken);
            }

            if (refreshToken != null)
            {
                await SecureStorage.SetAsync("refresh_token", refreshToken);
            }
        }

        public Task<string?> GetAuthTokenAsync() => SecureStorage.GetAsync( "auth_token" );

        public Task<string?> GetRefreshTokenAsync() => SecureStorage.GetAsync("refresh_token");
    }
}
