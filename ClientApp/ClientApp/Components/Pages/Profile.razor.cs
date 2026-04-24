using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages;

public partial class Profile : ComponentBase
{
    [Inject] private NavStateService NavStateService { get; set; } = null!;

    protected override void OnInitialized()
    {
        var nav = new NavState
        {
            CenterModalParameters = null,
            GoBackUrl = "/homepage"
        };
        NavStateService.UpdateNavState(nav);
    }
}