using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages;

public partial class ManageListPage : ComponentBase
{
    [Parameter] public int HouseId { get; set; }
    [Inject] private ListService ListService { get; set; } = null!;
    [Inject] private NavStateService NavStateService { get; set; } = null!;

    private List<UserList> Model { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        Model = await ListService.GetListsAsync(HouseId);
        
        NavStateService.UpdateNavState(new NavState
        {
            GoBackUrl = $"/homepage",
            CenterModalParameters = null,
            HouseId = HouseId
        });

        await base.OnInitializedAsync();
    }
}