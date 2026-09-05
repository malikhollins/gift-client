using ClientApp.Models;
using ClientApp.Utils;
using System.Net.Http.Json;

namespace ClientApp.Services
{
    public class PairService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PairService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<PairingData>> GenerateRandomPairingsAsync(int houseId)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var response = await httpClient.GetAsync($"/api/Pair/get/random?houseId={houseId}");
            response.EnsureSuccessStatusCode();
            return await response.DeserializeAsync<List<PairingData>>() ?? [];
        }

        public async Task<HttpResponseMessage> ApplyPairingsAsync(int houseId)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return await httpClient.PostAsync($"/api/Pair/set/pairs?houseId={houseId}", content: null);
        }

        public async Task<HttpResponseMessage> SetManualPairingsAsync(int houseId, IEnumerable<PairingData> pairings)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return await httpClient.PostAsJsonAsync($"/api/Pair/set/pairs/manual?houseId={houseId}", pairings);
        }

        public async Task<List<PairingData>> GetUserPairingAsync(int houseId)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var response = await httpClient.GetAsync($"/api/Pair/get/pairs/user?houseId={houseId}");
            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            return await response.DeserializeAsync<List<PairingData>>() ?? [];
        }
    }
}