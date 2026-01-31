using ClientApp.Models;
using ClientApp.Utils;
using SharedModels;
using System.Net.Http.Json;

namespace ClientApp.Services
{
    public class InviteService : BaseService
    {
        public InviteService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public Task CreateInvite(User userToInvite, House houseId)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return httpClient.PostAsJsonAsync("api/Invite", new { houseId = houseId.Id, userId = userToInvite.Id });
        }

        public async Task<List<UserInvites>> GetInvitesForUser( int userId )
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var response = await httpClient.GetAsync($"api/Invite/get/user/{userId}");
            return await DeserializeResponse.DeserializeAsync<List<UserInvites>>(response) ?? [];
        }

        public Task DeleteInvite(DeleteInviteRequest deleteInviteRequest)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return httpClient.PostAsJsonAsync("api/Invite/delete", deleteInviteRequest);
        }

        public Task RespondToUpdate( UpdateInviteRequest updateInviteRequest )
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            return httpClient.PostAsJsonAsync("api/Invite/update", updateInviteRequest);
        }

        public async Task<List<HouseInvites>> GetInvitesForHouse( int userId, int houseId )
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var response = await httpClient.GetAsync($"api/Invite/get/house/{userId}/{houseId}");
            return await DeserializeResponse.DeserializeAsync<List<HouseInvites>>(response) ?? [];
        }
    }
}
