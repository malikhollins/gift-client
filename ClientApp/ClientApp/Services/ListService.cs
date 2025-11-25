using ClientApp.Models;
using ClientApp.Utils;
using SharedModels;
using System.Net.Http.Json;

namespace ClientApp.Services
{
    public class ListService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ListService( IHttpClientFactory httpClientFactory  )
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<UserList>> GetListsAsync(int houseId)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var uri = $"/api/List/get/lists/{houseId}";
            var response = await httpClient.GetAsync(uri);
            var lists = await DeserializeResponse.DeserializeAsync<List<UserList>>(response);
            return lists ?? [];
        }

        public async Task CreateListAsync( CreateListRequest createListRequest)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            await httpClient.PostAsJsonAsync("/api/List/create/list", createListRequest);
        }
    }
}
