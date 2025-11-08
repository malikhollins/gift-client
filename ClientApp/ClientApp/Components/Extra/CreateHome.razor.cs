using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;

namespace ClientApp.Components.Extra
{
    public partial class CreateHome
    {
        [Inject] HouseService HouseService { get; set; } = null!;
        [Inject] UserInfoService UserInfoService { get; set; } = null!;
        public House HouseData { get; set; } = null!;
        public EditContext? EditContext { get; set; }
        public string MessageStore { get; set; } = "";

        private List<User> _usersToInvite =   [ new User { Email = "100" }, new User { Email = "105" }, new User { Email = "105" }, new User { Email = "105" }, new User { Email = "105" }];

        protected override void OnInitialized()
        {
            HouseData = new House();
            EditContext = new EditContext(HouseData);
        }

        public void OnUserInvited(User user)
        {
            if (!_usersToInvite.Contains(user))
            {
                _usersToInvite.Add(user);
            }
        }

        public async Task Submit()
        {
            MessageStore = $"{HouseData.Email} joined House {HouseData.Name}";

            var createHouseRequest = new CreateHouseRequest
            {
                InvitedUsers = _usersToInvite.Select( user => user.Id ).ToList(),
                Name = HouseData.Name ?? string.Empty,
                UserId = UserInfoService.GetUserInfo()?.Id ?? -1
            };

            var house = await HouseService.CreateHouseAsync(createHouseRequest);

            Console.WriteLine(house.ToString());

            // reset house data
            HouseData = new House();
            _usersToInvite.Clear();
        }
    }
}
