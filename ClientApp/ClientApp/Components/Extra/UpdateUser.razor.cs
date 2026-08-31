using BlazorBootstrap;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Utils;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
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
        
        [Inject]
        protected ToastService ToastService { get; set; } = null!;

        [Inject]
        public AuthTokenStorage AuthTokenStorage { get; set; } = null!;

        [Inject]
        public LoginService LoginService { get; set; } = null!;

        public User UserData { get; set; } = new();
        
        protected override async Task OnInitializedAsync()
        {
            var token = await AuthTokenStorage.GetValidAuthTokenAsync();
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;
                var authId = claims.FirstOrDefault(claim => claim.Type == "sub");
                if (authId != null)
                {
                    var user = await LoginService.GetUserAsync(claims);
                    if (user != null)
                    {
                        UserData = user;
                        UserInfoService.SetUserInfo(user);
                    }
                }
            }

            var userInfo = UserInfoService.GetUserInfo();
            if (userInfo!.Name.IsNullOrEmpty())
            {
                var mailAddress = new MailAddress(userInfo!.Email!);
                UserData.Name = mailAddress.User;
            }
            else
            {
                UserData.Name = userInfo.Name;
            }
        }

        public async Task HandleValidSubmit()
        {
            var successfulUpdate = await UserService.UpdateUserInfo(UserData!.Name!);
            if (successfulUpdate)
            {
                var user = UserInfoService.GetUserInfo();
                if (user != null)
                {
                    user.Name = UserData!.Name;
                    UserInfoService.SetUserInfo(user);
                    ToastService.Notify(new ToastMessage(ToastType.Success, "Changed name successfully!"));
                    return;
                }
            }
            ToastService.Notify(new ToastMessage(ToastType.Warning, "Failed to change name."));
        }
    }
}
