
using ClientApp.Components.Extra;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class Homepage
    {
        [Inject] HouseService HouseService { get; set; } = null!;
        [Inject] NavStateService NavStateService { get; set; } = null!;
        public IEnumerable<House> Houses { get; set; } = null!;
        public bool HasNoHouses => (Houses?.Count() ?? 0 ) == 0;
        private CenterModalParameters _centerModalParameters { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RefreshHousesAsync();
            await base.OnInitializedAsync();

           _centerModalParameters = new CenterModalParameters(
           typeof(CreateHome),
           "Create New House",
           EventCallback.Factory.Create(this, RefreshHousesAsync));
            NavStateService.UpdateNavState(new NavState()
            {
                CenterModalParameters = _centerModalParameters
            });
        }

        private async Task RefreshHousesAsync() => 
            Houses = await HouseService.GetHousesAsync();
    }
}
