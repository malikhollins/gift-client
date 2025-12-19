using ClientApp.Components.Extra;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientApp.Components.Pages
{
    public partial class HousePage : IAsyncDisposable
    {
        [Inject]
        public ListService ListService { get; set; } = null!;

        [Inject]
        public NavStateService NavStateService { get; set; } = null!;

        [Inject]
        public AuthTokenService AuthTokenService { get; set; } = null!;

        [Parameter]
        public int HouseId { get; set; }

        private List<UserList> _userLists { get; set; } = new();

        private CenterModalParameters _centerModalParameters;

        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _userLists = await ListService.GetListsAsync(HouseId);
            _centerModalParameters = new CenterModalParameters(
                typeof(CreateList),
                "Create New List",
                EventCallback.Factory.Create(this, RefreshListsAsync));
            NavStateService.UpdateNavState(new NavState 
            {
                GoBackUrl = "/homepage",
                CenterModalParameters = _centerModalParameters,
                HouseId = HouseId
            });

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://gift-application-service-garfg.ondigitalocean.app/apphub", options => 
                {
                    options.AccessTokenProvider = async () => await AuthTokenService.RefreshTokenAsync();
                })
                .Build();

            await _hubConnection.StartAsync();
            await _hubConnection.SendAsync("JoinHouse", HouseId);

            _hubConnection.On("NotifyListAdded", (object o) => 
            {
                if (o is UserList list)
                {
                    _userLists.Add(list);
                    StateHasChanged();
                }
            });
        }

        private async Task RefreshListsAsync() =>
            _userLists = await ListService.GetListsAsync(HouseId);

        public ValueTask DisposeAsync() => _hubConnection.DisposeAsync();
    }
}
