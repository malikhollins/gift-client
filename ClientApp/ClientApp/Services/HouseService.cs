
using ClientApp.Models;

namespace ClientApp.Services
{
    public class HouseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserInfoService _userInfoService;

        public HouseService( IHttpClientFactory httpClientFactory, UserInfoService userInfoService)
        {
            _httpClientFactory = httpClientFactory;
            _userInfoService = userInfoService;
        }

        public async Task GetUserHouseAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("base-url");
            User? user = _userInfoService.GetUserInfo();
            if (user == null)
            {
                return;
            }

            HttpResponseMessage response = await httpClient.GetAsync($"api/House/get/{user.Id}");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonResponse);
        }
    }
}
