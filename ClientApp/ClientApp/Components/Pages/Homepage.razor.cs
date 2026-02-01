
using ClientApp.Components.Extra;
using ClientApp.Components.Extra.Forms;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class Homepage : IDisposable
    {
        [Inject] HouseService HouseService { get; set; } = null!;
        [Inject] NavStateService NavStateService { get; set; } = null!;
        [Inject] HousePageObserver HomePageObserver { get; set; } = null!;
        public List<House> Houses { get; set; } = null!;
        public bool HasNoHouses => (Houses?.Count() ?? 0 ) == 0;
        private CenterModalParameters? _centerModalParameters { get; set; }

        private bool _isLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            await RefreshHousesAsync();
            await base.OnInitializedAsync();

           _centerModalParameters = new CenterModalParameters(
           typeof(CreateHome),
           "Create New House",
           EventCallback.Empty);

            NavStateService.UpdateNavState(new NavState()
            {
                CenterModalParameters = _centerModalParameters
            });

            _isLoading = false;

            HomePageObserver.OnHouseUpdated += HomePageObserver_OnHouseUpdated;
        }

        private void HomePageObserver_OnHouseUpdated(object? sender, House house)
        {
            Houses.Add(house);
            StateHasChanged();
        }

        private async Task RefreshHousesAsync()
        {
            var housesAsEnumerable = await HouseService.GetHousesAsync();
            Houses = housesAsEnumerable.ToList();
        }

        public void Dispose()
        {
            HomePageObserver.OnHouseUpdated -= HomePageObserver_OnHouseUpdated;
        }
    }
}
