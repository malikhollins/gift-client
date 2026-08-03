using ClientApp.Components.Extra.Forms;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientApp.Components.Pages
{
    public partial class Homepage : IDisposable, IAsyncDisposable
    {
        [Inject] private HouseService HouseService { get; set; } = null!;
        [Inject] private NavStateService NavStateService { get; set; } = null!;
        [Inject] private HousePageObserver HomePageObserver { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;
        [Inject] private HubService HubService { get; set; } = null!;
        private List<House> Houses { get; set; } = null!;
        private bool HasNoHouses => (Houses?.Count ?? 0 ) == 0;
        private CenterModalParameters? CenterModalParameters { get; set; }
        private bool IsLoading { get; set; }        
        private HubConnection? Connection { get; set; }
        private bool housesOrListsChanged = false;
        
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
            
            RegisterHouseConnection().SafeFireAndForget(); // we don't need to stop rendering for this
        }
        
        private async Task RegisterHouseConnection()
        {
            var user = UserInfoService.GetUserInfo();
            if (user == null)
            {
                return;
            }

            Connection ??= HubService.Build();
            await Connection.SendAsync("JoinHouses",  user.Id, Houses );

            Connection.On("NotifyListCreated", () =>
            {
                housesOrListsChanged = true;
            });

            Connection.On("NotifyListDeleted", ( object obj ) =>
            {
                housesOrListsChanged = true;
                
                if (obj is not int listId)
                {
                    return;
                }
                
                foreach (var house in Houses)
                {
                    foreach (var list in house.Lists)
                    {
                        if (list.ListId == listId)
                        {
                            list.Deleted = true;
                        }
                    }
                }
            });
        }

        private async Task<bool> ShouldRefreshHousesAsync()
        {
            if (!housesOrListsChanged)
            {
                return false;
            }
            
            await RefreshHousesAsync();

            housesOrListsChanged = false;
            
            return true;
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

        public async ValueTask DisposeAsync()
        {
            if (Connection != null) await Connection.DisposeAsync();
        }
    }
}
