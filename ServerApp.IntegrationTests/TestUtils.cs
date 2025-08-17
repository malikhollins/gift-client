using ServerApp.Models;
using WebAPI.Models;

namespace ServerApp.IntegrationTests
{
    public static class TestUtils
    {
        private static House? CreatedHouse { get; set; }

        private static User? CreatedUser { get; set; }

        public static async Task<(House, User)> CreateHouseAsync()
        {
            if (CreatedHouse != null && CreatedUser != null)
            { 
                return (CreatedHouse, CreatedUser);
            }
            var houseService = HouseTests.SetupHouseService();
            CreatedUser = await HouseTests.CreateDummyUserAsync();
            CreatedHouse = await houseService.CreateHouseAsync(CreatedUser.Id, "hello");
            return (CreatedHouse, CreatedUser);
        }
    }
}
