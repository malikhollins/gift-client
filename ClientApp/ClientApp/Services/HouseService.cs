
using ClientApp.Models;
using ClientApp.Utils;
using System.Text.Json;

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

        public async Task<IEnumerable<House>> GetHousesAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("base-url");
            User? user = _userInfoService.GetUserInfo();
            HttpResponseMessage response = await httpClient.GetAsync($"api/House/get/{user?.Id ?? -1}");
            return await response.DeserializeAsync<IEnumerable<House>>() ?? [];
        }

        public async Task<House> CreateHouseAsync( House houseToCreate )
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("base-url");
            User? user = _userInfoService.GetUserInfo();
            HttpResponseMessage response = await httpClient.GetAsync($"api/House/create/{user?.Id ?? -1}/{houseToCreate.Name}");
            return await response.DeserializeAsync<House>() ?? new House();
        }
    }
}
