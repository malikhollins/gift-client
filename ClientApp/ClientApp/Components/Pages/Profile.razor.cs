using BlazorBootstrap;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages;

public partial class Profile : ComponentBase
{
    [Inject] private NavStateService NavStateService { get; set; } = null!;
    [Inject] private UserInfoService UserInfoService { get; set; } = null!;
    [Inject] private ToastService ToastService { get; set; } = null!;

    protected override void OnInitialized()
    {
        var user = UserInfoService.GetUserInfo();
        if (string.IsNullOrWhiteSpace(user?.Name))
        {
            ToastService.Notify(new ToastMessage(ToastType.Info, "Please enter your name to finish setting up your account."));
        }

        var nav = new NavState
        {
            CenterModalParameters = null,
            CurrentPage = NavPage.Profile
        };
        NavStateService.UpdateNavState(nav);
    }
}