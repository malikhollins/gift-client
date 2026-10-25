using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class BottomNav : IDisposable
    {
        [Inject] NavStateService NavStateService { get; set; } = null!;
        [Inject] InviteService InviteService { get; set; } = null!;
        [Inject] UserInfoService UserInfoService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Parameter] public CenterModalParameters? CenterModalParameters { get; set; }
        private int HouseId { get; set; }
        private int ListId { get; set; }
        private bool HasInvites { get; set; }
        private NavPage CurrentPage { get; set; }

        protected override Task OnInitializedAsync()
        {
            NavStateService.OnNavStateChanged += NavStateService_OnNavStateChanged;
            UpdateNavParameters( NavStateService!.CurrentState! );
            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            var user = UserInfoService.GetUserInfo();
            if (user != null)
            {
                var inviteResponse = await InviteService.GetInvitesForUser(user.Id);
                HasInvites = inviteResponse.Any(invite => invite.InviteStatus == InviteStatus.Pending);
            }
            else
            {
                HasInvites = false;
            }
        }

        private void NavStateService_OnNavStateChanged(object? sender, NavState? naveState)
            => UpdateNavParameters( naveState );

        private void UpdateNavParameters(NavState? state)
        {
            if (state == null)
            {
                return;
            }

            CenterModalParameters = state.CenterModalParameters;
            HouseId = state.HouseId;
            ListId = state.ListId;
            CurrentPage = state.CurrentPage;
            StateHasChanged();
        }

        private void NavigateHome() => NavigationManager.NavigateTo("/homepage");

        private void NavigateToProfile() => NavigationManager.NavigateTo("/profile");

        public void Dispose()
        {
            NavStateService.OnNavStateChanged -= NavStateService_OnNavStateChanged;
        }
    }
}