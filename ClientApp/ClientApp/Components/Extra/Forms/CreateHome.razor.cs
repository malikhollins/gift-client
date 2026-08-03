using ClientApp.Components.Pages;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;

namespace ClientApp.Components.Extra.Forms
{
    public partial class CreateHome
    {
        [Inject] private HouseService HouseService { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;
        [Inject] private HousePageObserver HousePageObserver { get; set; } = null!;
        [CascadingParameter] public EventCallback OnSubmitCompleted { get; set; } = default!;
        
        public House HouseData { get; set; } = null!;
        public EditContext? EditContext { get; set; }

        private readonly List<User> _usersToInvite = [];
        private bool _submitting;

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
        
        private async Task Submit()
        {
            _submitting = true;

            var createHouseRequest = new CreateHouseRequest
            {
                InvitedUsers = [.. _usersToInvite.Select( user => user.Id )],
                Name = HouseData.Name ?? string.Empty,
                UserId = UserInfoService.GetUserInfo()?.Id ?? -1
            };

            var house = await HouseService.CreateHouseAsync(createHouseRequest);
            house.Lists ??= [];
            var update = new UpdateEventHouseArgs( house, UpdateEventType.Add );
            HousePageObserver.NotifyUpdated(update);
            
            // reset house data
            HouseData = new House();
            _usersToInvite.Clear();

            await OnSubmitCompleted.InvokeAsync();

            _submitting = false;
        }
    }
}
