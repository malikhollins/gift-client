using Auth0.OidcClient;
using System.Diagnostics;

namespace ClientApp.Services
{
    public class LoginService
    {
        private readonly Auth0Client _authClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginService(Auth0Client client, IHttpClientFactory httpClientFactory )
        {
            _authClient = client;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> LoginAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient( "base-url" );

            try
            {
                var loginResult = await _authClient.LoginAsync();


                if (loginResult.IsError)
                {
                    return false;
                }

                var wasCreated = loginResult.User.Claims.FirstOrDefault( claim => claim.Type == "app_metadata");
                var userId = loginResult.User.Claims.FirstOrDefault(claim => claim.Type == "sub");

                if (wasCreated != null && (bool.TryParse(wasCreated.Value, out var result) && result))
                {
                    var email = loginResult.User.Claims.FirstOrDefault(claim => claim.Type == "email");
                    var response = await httpClient.GetAsync($"api/User/create/{userId}/{email}");
                    Debug.WriteLine(response);
                }
                else
                {
                    // fetch db
                    var response = await httpClient.GetAsync($"api/User/get/{userId}");
                    Debug.WriteLine(response);
                }

                return true;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

    }
}
