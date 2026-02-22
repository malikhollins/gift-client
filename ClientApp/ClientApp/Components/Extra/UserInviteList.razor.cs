using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientApp.Components.Extra
{
    partial class UserInviteList : IAsyncDisposable
    {
        [Inject] private InviteService InviteService { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;
        [Inject] private HubService HubService { get; set; } = null!;
        private List<UserInvites> Invites { get; set; } = [];
        
        private HubConnection? _hubConnection;
        
        protected override async Task OnInitializedAsync()
        {
            var user = UserInfoService.GetUserInfo();
            Invites = await InviteService.GetInvitesForUser(user!.Id);

            _hubConnection = HubService.Build();
            
            await _hubConnection.StartAsync();
            
            _hubConnection.On("NotifyInviteReceived", (object o) => 
            {
                if (o is UserInvites invite)
                {
                    Invites.Add(invite);
                    StateHasChanged();
                }
            });
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null) await _hubConnection.DisposeAsync();
        }
    }
}
