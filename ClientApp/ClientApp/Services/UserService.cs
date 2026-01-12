using ClientApp.Models;
using ClientApp.Utils;
using System.Net.Http.Json;

namespace ClientApp.Services
{
    public class UserService : BaseService
    {
        public UserService( IHttpClientFactory httpClientFactory ) : base( httpClientFactory )
        {
        }

        private record NameUpdateRequest(string name);

        public async Task UpdateUserInfo(string name)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var uri = $"api/User/update/name?name={Uri.EscapeDataString(name)}";
            var response = await httpClient.PostAsync(uri, null); 
            Console.Write(response.ToString());
        }

        public async Task<List<User>> BulkGetUsersAsync(string input, CancellationToken cancellationToken = default)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var uri = $"api/User/get/bulk/{input}";

            var response = await httpClient.GetAsync(uri, cancellationToken );
            if (response == null)
            {
                return new List<User>();
            }

            var list = await response.DeserializeAsync<List<User>>();
            if ( list == null )
            {
                return new List<User>();
            }

            return list;
        }
    }
}
