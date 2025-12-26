using ClientApp.Models;
using ClientApp.Utils;

namespace ClientApp.Services
{
    public class UserService : BaseService
    {
        public UserService( IHttpClientFactory httpClientFactory ) : base( httpClientFactory )
        {
        }

        public async Task UpdateUserInfo(string name)
        {
            var httpClient = _httpClientFactory.CreateClient("base-url");
            var uri = $"api/User/update/name/{name}";
            await httpClient.PutAsync(uri, null);
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
