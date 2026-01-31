using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class BottomNav : IDisposable
    {
        [Inject] NavStateService NavStateService { get; set; } = null!;
        [Parameter] public string? GoBackUrl { get; set; }
        [Parameter] public CenterModalParameters CenterModalParameters { get; set; } = default!;
        private int HouseId { get; set; }
        private int ListId { get; set; }

        protected override Task OnInitializedAsync()
        {
            NavStateService.OnNavStateChanged += NavStateService_OnNavStateChanged;
            UpdateNavParameters( NavStateService!.CurrentState! );
            return base.OnInitializedAsync();
        }

        private void NavStateService_OnNavStateChanged(object? sender, NavState naveState )
            => UpdateNavParameters( naveState );

        private void UpdateNavParameters( NavState state )
        {
            if (state == null)
            {
                return;
            }

            GoBackUrl = state.GoBackUrl;
            CenterModalParameters = state!.CenterModalParameters!;
            HouseId = state.HouseId;
            ListId = state.ListId;
            StateHasChanged();
        }

        public void Dispose()
        {
            NavStateService.OnNavStateChanged -= NavStateService_OnNavStateChanged;
        }
    }
}