using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Components.Extra.Confirmation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SharedModels;

namespace ClientApp.Components.Extra
{
    partial class UserInviteList : IAsyncDisposable
    {
        [Inject] private InviteService InviteService { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;
        [Inject] private HubService HubService { get; set; } = null!;
        [Inject] private HousePageObserver HousePageObserver { get; set; } = null!;
        private List<UserInvites> Invites { get; set; } = [];
        private UniversalModal? _leaveHouseModal;
        private CenterModalParameters? _leaveHouseParameters;
        private UserInvites? _inviteToLeave;

        private List<UserInvites> PendingInvites => Invites.Where(invite => invite.InviteStatus == InviteStatus.Pending).ToList();
        private List<UserInvites> AcceptedInvites => Invites.Where(invite => invite.InviteStatus == InviteStatus.Accepted).ToList();
        
        private HubConnection? _hubConnection;
        
        protected override async Task OnInitializedAsync()
        {
            var user = UserInfoService.GetUserInfo();
            Invites = await InviteService.GetInvitesForUser(user!.Id);

            _hubConnection = HubService.Build();
            
            await _hubConnection.StartAsync();
            
            _hubConnection.On("NotifyInviteReceived", (object o) => 
            {
                if (o is UserInvites invite && invite.InviteStatus == InviteStatus.Pending)
                {
                    Invites.Add(invite);
                    StateHasChanged();
                }
            });
        }

        private async Task ConfirmLeaveHouseAsync(UserInvites invite)
        {
            _inviteToLeave = invite;
            _leaveHouseParameters = new CenterModalParameters(
                typeof(LeaveHouseConfirmation),
                $"Leave {invite.House?.Name}",
                EventCallback.Factory.Create(this, LeaveHouseAsync));

            if (_leaveHouseModal != null)
            {
                await _leaveHouseModal.ShowModalAsync();
            }
        }

        private async Task LeaveHouseAsync()
        {
            var user = UserInfoService.GetUserInfo();
            if (user == null || _inviteToLeave?.House == null)
            {
                return;
            }

            var response = await InviteService.RespondToUpdate(new UpdateInviteRequest
            {
                HouseId = _inviteToLeave.House.Id,
                Status = (int)InviteStatus.Rejected,
                UserId = user.Id
            });

            if (response.IsSuccessStatusCode)
            {
                var house = _inviteToLeave.House;
                Invites.Remove(_inviteToLeave);
                _inviteToLeave = null;
                HousePageObserver.NotifyUpdated(new UpdateEventHouseArgs(house, UpdateEventType.Delete));
                StateHasChanged();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null) await _hubConnection.DisposeAsync();
        }
    }
}
