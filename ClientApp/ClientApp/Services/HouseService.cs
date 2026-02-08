using ClientApp.Models;
using ClientApp.Utils;
using Models;
using System.Net.Http.Json;

namespace ClientApp.Services
{
    public class HouseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HouseService( IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<House>> GetHousesAsync()
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("base-url");
                HttpResponseMessage response = await httpClient.GetAsync($"api/House/get/v3/");
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
