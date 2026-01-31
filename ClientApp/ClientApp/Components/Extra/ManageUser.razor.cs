using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using SharedModels;

namespace ClientApp.Components.Extra
{
    public partial class ManageUser
    {
        [Inject] InviteService InviteService { get; set; } = null!;

        [Inject] UserInfoService UserInfoService { get; set; } = null!;

        [Parameter] public HouseInvites Model { get; set; } = null!;

        [Parameter] public int HouseId { get; set; }

        private bool wasDeleted = false;

        public async Task DeleteUserFromHouseAsync()
        {
            var user = UserInfoService.GetUserInfo();

            var request = new DeleteInviteRequest
            {
                HouseId = HouseId,
                UserId = user!.Id,
                UserInviteToDelete = Model.User!.Id
            };

            var response = InviteService.DeleteInvite(request);
            if (response.IsCompletedSuccessfully)
            {
                wasDeleted = true;
                StateHasChanged();
            }
        }
    }
}
