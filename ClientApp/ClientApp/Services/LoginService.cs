using Auth0.OidcClient;
using ClientApp.Models;
using System.Diagnostics;
using System.Text.Json;

namespace ClientApp.Services
{
    public class LoginService
    {
        private readonly Auth0Client _authClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserInfoService _userInfoService;

        public LoginService(Auth0Client client, IHttpClientFactory httpClientFactory, UserInfoService userInfoService )
        {
            _authClient = client;
            _httpClientFactory = httpClientFactory;
            _userInfoService = userInfoService;
        }

        public async Task<bool> LoginAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient( "base-url" );

            try
            {
                var loginResult = await _authClient.LoginAsync();
                 if (loginResult.IsError)
                {
                    // auth0 doesn't throw exceptions, throw it for them
                    throw new Exception(loginResult.Error);
                }

                var authId = loginResult.User.Claims.FirstOrDefault(claim => claim.Type == "sub");
                var email = loginResult.User.Claims.FirstOrDefault(claim => claim.Type == "name");
                HttpResponseMessage response = await httpClient.GetAsync($"api/User/get/{authId.Value}/{email.Value}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine( $"Status Code: {response.StatusCode} with message: {response.RequestMessage}" );
                }
                string json = await response.Content.ReadAsStringAsync();
                User? user = JsonSerializer.Deserialize<User>(json ?? string.Empty);

                if (user != null)
                {
                    _userInfoService.SetUserInfo(user);
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to parse content");
                    return false;
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

    }
}
