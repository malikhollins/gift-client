
using ClientApp.Components.Extra;
using ClientApp.Components.Extra.Forms;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class Homepage : IDisposable
    {
        [Inject] private HouseService HouseService { get; set; } = null!;
        [Inject] private NavStateService NavStateService { get; set; } = null!;
        [Inject] private HousePageObserver HomePageObserver { get; set; } = null!;
        private List<House> Houses { get; set; } = null!;
        private bool HasNoHouses => (Houses?.Count ?? 0 ) == 0;
        private CenterModalParameters? CenterModalParameters { get; set; }
        private bool IsLoading { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            await RefreshHousesAsync();
            await base.OnInitializedAsync();

           CenterModalParameters = new CenterModalParameters(
           typeof(CreateHome),
           "Create New House",
           EventCallback.Empty);

            NavStateService.UpdateNavState(new NavState()
            {
                CenterModalParameters = CenterModalParameters
            });

            IsLoading = false;

            HomePageObserver.OnUpdated += HomePageObserver_OnHouseUpdated;
        }
        
        private void HomePageObserver_OnHouseUpdated(object? sender, UpdateEventHouseArgs args)
        {
            switch (args.UpdateEventType)
            {
                case UpdateEventType.Delete:
                    Houses.RemoveAll( h  => h.Id == args.House.Id );
                    break;
                case UpdateEventType.Add:
                    Houses.Add(args.House);
                    break;
            }
            StateHasChanged();
        }
        
        private async Task RefreshHousesAsync()
        {
            var housesAsEnumerable = await HouseService.GetHousesAsync();
            Houses = housesAsEnumerable.ToList();
        }

        public void Dispose()
        {
            HomePageObserver.OnUpdated -= HomePageObserver_OnHouseUpdated;
        }
    }
}
