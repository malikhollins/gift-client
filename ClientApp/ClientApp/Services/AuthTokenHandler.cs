namespace ClientApp.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly AuthTokenService _authTokenService;
        
        public AuthTokenHandler(AuthTokenService authTokenService)
        {
            _authTokenService = authTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authTokenService.RefreshTokenAsync();
            if (token != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }   
    }
}
