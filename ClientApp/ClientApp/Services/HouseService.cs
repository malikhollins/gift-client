
namespace ClientApp.Services
{
    public class HouseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HouseService( IHttpClientFactory httpClientFactory ) 
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task GetUserHouseAsync(int userId)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("base-url");

            using HttpResponseMessage response = await httpClient.GetAsync($"api/House/get/{userId}");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine(jsonResponse);
        }
    }
}
