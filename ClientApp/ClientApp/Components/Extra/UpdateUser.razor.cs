using BlazorBootstrap;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;

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

        [Inject]
        public NavStateService NavStateService { get; set; } = null!;

        public User UserData { get; set; } = new();

        protected override void OnInitialized() => UserData = UserInfoService.GetUserInfo();

        public async Task HandleValidSubmit()
        {
            var name = UserData.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                ToastService.Notify(new ToastMessage(ToastType.Warning, "Please enter your name to continue."));
                return;
            }

            var successfulUpdate = await UserService.UpdateUserInfo(name);
            if (successfulUpdate)
            {
                var user = UserInfoService.GetUserInfo();
                if (user != null)
                {
                    user.Name = name;
                    UserInfoService.SetUserInfo(user);
                }

                if (NavStateService.CurrentState is { } navState)
                {
                    NavStateService.UpdateNavState(navState);
                }

                ToastService.Notify(new ToastMessage(ToastType.Success, "Changed name successfully!"));
                return;
            }
            ToastService.Notify(new ToastMessage(ToastType.Warning, "Failed to change name."));
        }
    }
}
