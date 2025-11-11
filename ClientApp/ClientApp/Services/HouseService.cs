
using ClientApp.Models;
using ClientApp.Utils;
using Cysharp.Web;
using Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("base-url");
                User? user = _userInfoService.GetUserInfo();
                HttpResponseMessage response = await httpClient.GetAsync($"api/House/get/{user?.Id ?? -1}");
                return await response.DeserializeAsync<IEnumerable<House>>() ?? [];
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return [];
            }
        }

        public async Task<House> CreateHouseAsync( CreateHouseRequest createHouseRequest )
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("base-url");
            var uri = "api/House/create";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync( uri, createHouseRequest);
            return await response.DeserializeAsync<House>() ?? new House();
        }
    }
}
