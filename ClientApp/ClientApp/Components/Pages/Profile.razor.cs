using BlazorBootstrap;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClientApp.Components.Pages;

public partial class Profile : ComponentBase
{
    [Inject] private NavStateService NavStateService { get; set; } = null!;
    [Inject] private UserInfoService UserInfoService { get; set; } = null!;
    [Inject] private ToastService ToastService { get; set; } = null!;

    [Inject] private IJSRuntime JS { get; set; } = null!;

    protected string AppearancePreference { get; set; } = "system";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        try
        {
            AppearancePreference = await JS.InvokeAsync<string>("giftAllTheme.getPreference");
            StateHasChanged();
        }
        catch (JSException) {}
    }

    protected async Task SetAppearanceAsync(string preference)
    {
        if (preference is not ("system" or "light" or "dark")) return;
        try
        {
            await JS.InvokeVoidAsync("giftAllTheme.setPreference", preference);
            AppearancePreference = preference;
        }
        catch (JSException) {}
    }

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