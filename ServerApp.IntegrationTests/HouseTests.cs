using ServerApp.Models;
using ServerApp.Services;
using WebAPI.Services;

namespace ServerApp.IntegrationTests
{
    public class HouseTests
    {
        public static HouseService SetupHouseService()
        {
            var connectionService = ConnectionTests.SetUpConnection();
            return new HouseService(connectionService);
        }

        public static Task<User> CreateDummyUserAsync()
        {
            var connectionService = ConnectionTests.SetUpConnection();
            var userService = new UserService(connectionService);
            return userService.CreateUserAsync("authid", "email" )!;
        }

        [Fact]
        public async Task CreateHouseTest()
        {
            var houseService = SetupHouseService();
            var user = await CreateDummyUserAsync();
            var house = await houseService.CreateHouseAsync( user.Id, "test" );
            Assert.NotNull( house );
            Assert.True(house.Name == "test");
            Assert.True(house.GiftType == 0);
        }

        [Fact]
        public async Task GetHouseTest()
        {
            var houseService = SetupHouseService();
            var user = await CreateDummyUserAsync();
            var house = await houseService.CreateHouseAsync(user.Id, "test");
            var houses = await houseService.GetHousesAsync(user.Id);
            Assert.NotNull(houses);
            var createdHouse = houses.FirstOrDefault();
            Assert.NotNull(createdHouse);
            Assert.True(createdHouse.Name == house.Name);
            Assert.True(createdHouse.Id == house.Id);
            Assert.True(createdHouse.GiftType == house.GiftType);
        }
    }
}
