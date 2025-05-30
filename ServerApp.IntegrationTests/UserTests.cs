using ServerApp.Services;

namespace ServerApp.IntegrationTests
{
    public class UserTests
    {
        private const string TestEmail = "testemail@gmail.com";

        private static UserService SetupUserService()
        {
            var connectionService = ConnectionTests.SetUpConnection();
            return new UserService(connectionService);
        }

        [Fact]
        public async Task Create_User()
        {
            var userService = SetupUserService();
            var user = await userService.CreateUserAsync(authId: "email|67ee8811faba3e9e52a277d9", TestEmail);
            Assert.NotNull(user);
            Assert.True(user.Email == TestEmail);
        }

        [Fact]
        public async Task Get_Created_User()
        {
            var userService = SetupUserService();
            await userService.CreateUserAsync(authId: "email|67ee8811faba3e9e52a277d9", TestEmail); // create user

            var user = await userService.GetUserAsync("email|67ee8811faba3e9e52a277d9"); // try to get the user from db
            Assert.NotNull(user);
            Assert.True(user.Email == TestEmail);
        }


        [Fact]
        public async Task Get_Created_User_Not_Found()
        {
            var userService = SetupUserService();
            var user = await userService.GetUserAsync("email|adkiakda"); // try to get fake user from db
            Assert.Null(user);
        }
    }
}
