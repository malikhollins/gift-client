using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Utils;
using Microsoft.AspNetCore.Components;
using System.Net.Mail;

namespace ClientApp.Components.Extra
{
    public partial class UpdateUser
    {
        [Inject]
        public UserService UserService { get; set; } = null!;

        [Inject]
        public UserInfoService UserInfoService { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        public User UserData { get; set; } = new();

        protected override void OnInitialized()
        {
            var userInfo = UserInfoService.GetUserInfo();
            if (userInfo.Name.IsNullOrEmpty())
            {
                var mailAddress = new MailAddress(userInfo.Email);
                UserData.Name = mailAddress.User;
            }
            else
            {
                UserData.Name = userInfo.Name;
            }
        }

        public async Task HandleValidSubmit()
        {
            await UserService.UpdateUserInfo(UserData.Name);

            NavigationManager.NavigateTo("/homepage");
        }
    }
}
