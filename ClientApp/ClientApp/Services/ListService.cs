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

        public async Task CreateItemAsync( CreateItemRequest createItemRequest)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            await httpClient.PostAsJsonAsync("/api/List/create/item", createItemRequest);
        }

        public async Task<HttpResponseMessage> UpdateBuyerAsync( UpdateBuyerInItemRequest updateBuyerInListRequest )
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return await httpClient.PostAsJsonAsync("/api/List/update/item/buyer", updateBuyerInListRequest);
        }

        public async Task<HttpResponseMessage> UpdateFavoriteInList( UpdateFavoriteItemRequest updateFavoriteItem )
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return await httpClient.PostAsJsonAsync("/api/List/update/item/favorite", updateFavoriteItem);
        }

        public async Task<HttpResponseMessage> UpdateItemAsync( UpdateItemRequest updateItemRequest)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return await httpClient.PostAsJsonAsync("/api/List/update/item", updateItemRequest);
        }
        
        public async Task DeleteListAsync( int listId )
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            await httpClient.DeleteAsync($"/api/List/delete/list/{listId}");
        }

        public async Task<HttpResponseMessage> DeleteItemAsync(int listId, int itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return await httpClient.DeleteAsync($"/api/List/delete/item/{listId}/{itemId}");
        }

        public async Task<List<Item>> GetItemsAsync(int listId)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var response = await httpClient.GetAsync($"/api/List/get/items/{listId}");
            return await DeserializeResponse.DeserializeAsync<List<Item>>(response) ?? new List<Item>();
        }
    }
}
