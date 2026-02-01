using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    partial class UserInviteList
    {
        [Inject] private InviteService InviteService { get; set; } = null!;

        [Inject] private UserInfoService UserInfoService { get; set; } = null!;

        public List<UserInvites> Invites { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            var user = UserInfoService.GetUserInfo();
            Invites = await InviteService.GetInvitesForUser(user!.Id);
        }
    }
}
