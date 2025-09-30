using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;

namespace ClientApp.Components.Pages
{
    public partial class CreateHome
    {
        [Inject] HouseService HouseService { get; set; } = null!;
        [Inject] UserInfoService UserInfoService { get; set; } = null!;
        public House HouseData { get; set; } = null!;
        public EditContext? EditContext { get; set; }
        public string MessageStore { get; set; } = "";

        private List<int> _usersToInvite = new();

        protected override void OnInitialized()
        {
            HouseData = new House();
            EditContext = new EditContext(HouseData);
        }

        public void OnUserInvited(User user)
        {
            if (!_usersToInvite.Contains(user.Id))
            {
                _usersToInvite.Add(user.Id);
            }
        }

        public async Task Submit()
        {
            MessageStore = $"{HouseData.Email} joined House {HouseData.Name}";

            var createHouseRequest = new CreateHouseRequest
            {
                InvitedUsers = _usersToInvite,
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
