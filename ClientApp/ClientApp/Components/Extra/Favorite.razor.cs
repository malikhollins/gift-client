using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using SharedModels;

namespace ClientApp.Components.Extra
{
    partial class Favorite
    {
        [Parameter] public Item Item { get; set; } = null!;
        [Parameter] public int ListId { get; set; }
        [Parameter] public bool CanToggle { get; set; } = true;
        [Parameter] public Action<bool> OnFavoriteToggled { get; set; } = null!;
        [Inject] private ListService ListService { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;

        private bool isFavorited;

        protected override void OnParametersSet()
        {
            isFavorited = Item.Favorite;
        }

        private async Task ToggleFavoriteAsync()
        {
            var user = UserInfoService.GetUserInfo();
            if (user == null)
            {
                return;
            }

            var request = new UpdateFavoriteItem
            {
                ItemId = Item.Id,
                ListId = ListId,
                UserId = user.Id,
                Favorited = !Item.Favorite
            };

            var response = await ListService.UpdateFavoriteInList(request);
            if ( response.IsSuccessStatusCode )
            {
                isFavorited = !isFavorited;
                OnFavoriteToggled.Invoke(isFavorited);
                StateHasChanged();
            }
        }
    }
}
