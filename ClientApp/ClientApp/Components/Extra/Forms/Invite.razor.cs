using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra.Forms
{
    public partial class Invite
    {
        [CascadingParameter( Name = "HouseId" )] public int HouseId { get; set; }

        [Inject] private InviteService InviteService { get; set; } = null!;

        [Inject] private UserInfoService UserInfoService { get; set; } = null!;

        private List<HouseInvites> InvitedUsers { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            var user = UserInfoService.GetUserInfo();
            InvitedUsers = await InviteService.GetInvitesForHouse( user!.Id, HouseId);
        }

        public async Task OnUserInvitedAsync(User user)
        {
            var activeUser = UserInfoService.GetUserInfo();
            var response = await InviteService.CreateInvite(activeUser!.Id, user, HouseId);
            if (response.IsSuccessStatusCode)
            {
                InvitedUsers.Add(new HouseInvites
                {
                    User = user,
                    InviteStatus = InviteStatus.Pending
                });
                StateHasChanged();
            }
        }
    }
}
