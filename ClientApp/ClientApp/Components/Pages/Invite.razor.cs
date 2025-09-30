using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class Invite
    {
        [Inject] private InviteService InviteService { get; set; } = null!;

        [Inject] private UserInfoService UserInfoService { get; set; } = null!;

        public void OnUserInvited( User user )
        {
            var activeHouse = UserInfoService.GetHouseInfo();

            if ( activeHouse == null )
            {
                return;
            }

            InviteService.CreateInvite(user, activeHouse);
            Console.WriteLine($"User invited: {user.Email}");
        }
    }
}
