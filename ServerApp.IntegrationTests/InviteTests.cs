using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.IntegrationTests
{
    public class InviteTests
    {
        private static InviteService SetupInviteService()
        {
            var connectionService = ConnectionTests.SetUpConnection();
            return new InviteService(connectionService);
        }

        [Fact]
        public async Task CreateHouseInvite_NoError()
        {
            var houseService = HouseTests.SetupHouseService();
            var userService = UserTests.SetupUserService();
            var inviteService = SetupInviteService();

            // setup dependecies
            var userA = await userService.CreateUserAsync("test", "email");
            var userB = await userService.CreateUserAsync("test2", "email2");
            var house = await houseService.CreateHouseAsync(userA!.Id, "temp");

            // invite a user to a house
            await inviteService.CreateInvite(house.Id, userB!.Id);

            // house invites now has the invite for the user
            var houseInvites = await inviteService.GetInvitesForHouseAsync(house.Id);
            Assert.NotNull(houseInvites);
            var nonAcceptedHouse = houseInvites.FirstOrDefault();
            Assert.NotNull(nonAcceptedHouse);
            Assert.NotNull(nonAcceptedHouse.User);
            Assert.True(userB!.Id == nonAcceptedHouse.User.Id);
            Assert.True(InviteStatus.Pending == nonAcceptedHouse.InviteStatus);
        }


        [Fact]
        public async Task GetUserInvite_NoError()
        {
            var houseService = HouseTests.SetupHouseService();
            var userService = UserTests.SetupUserService();
            var inviteService = SetupInviteService();

            // setup dependecies
            var userA = await userService.CreateUserAsync("test", "email");
            var userB = await userService.CreateUserAsync("test2", "email2");
            var house = await houseService.CreateHouseAsync(userA!.Id, "temp");
            await inviteService.CreateInvite(house.Id, userB!.Id);
            var userInvites = await inviteService.GetInvitesForUserAsync(userB.Id);
            Assert.NotNull(userInvites);
            var nonAcceptedUser = userInvites.FirstOrDefault();
            Assert.NotNull(nonAcceptedUser);
            Assert.NotNull(nonAcceptedUser.House);
            Assert.True(house.Id == nonAcceptedUser.House.Id);
            Assert.True(InviteStatus.Pending == nonAcceptedUser.InviteStatus);
        }

        [Fact]
        public async Task UpdateUserInvite_NoError()
        {
            var houseService = HouseTests.SetupHouseService();
            var userService = UserTests.SetupUserService();
            var inviteService = SetupInviteService();

            // setup dependecies
            var userA = await userService.CreateUserAsync("test", "email");
            var userB = await userService.CreateUserAsync("test2", "email2");
            var house = await houseService.CreateHouseAsync(userA!.Id, "temp");
            await inviteService.CreateInvite(house.Id, userB!.Id);
            await inviteService.UpdateInvite(house.Id, userB!.Id, (int) InviteStatus.Rejected);
            var userInvites = await inviteService.GetInvitesForUserAsync(userB.Id);
            Assert.NotNull(userInvites);
            var nonAcceptedUser = userInvites.FirstOrDefault();
            Assert.NotNull(nonAcceptedUser);
            Assert.NotNull(nonAcceptedUser.House);
            Assert.True(house.Id == nonAcceptedUser.House.Id);
            Assert.True(InviteStatus.Rejected == nonAcceptedUser.InviteStatus);
        }
    }
}
