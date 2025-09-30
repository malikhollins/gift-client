using ClientApp.Models;
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
    }
}
