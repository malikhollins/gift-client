using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using SharedModels;

namespace ClientApp.Components.Extra
{
    partial class Buyer
    {
        [Parameter] public Item Item { get; set; } = null!;
        [Parameter] public int ListId { get; set; }
        [Inject] private ListService ListService { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;

        private bool didUserBuy;
        private bool isPurchased;
        private bool canInteract;

        protected override void OnParametersSet() => RefreshButton();

        private void RefreshButton()
        {
            var user = UserInfoService.GetUserInfo();
            didUserBuy = Item.Buyer == user!.Id;
            isPurchased = Item.Buyer != null;
            canInteract = didUserBuy || !isPurchased;
        }

        private async Task MarkAsBuyerAsync()
        {
            var user = UserInfoService.GetUserInfo();
            if (user == null)
            {
                return;
            }

            int? buyerId = user.Id == Item.Buyer ? null : user.Id;
            var updateBuyerRequest = new UpdateBuyerInItemRequest
            {
                UserId = user.Id,
                ItemId = Item.Id,
                ListId = ListId,
                BuyerId = buyerId
            };

            var response = await ListService.UpdateBuyerAsync(updateBuyerRequest);
            if ( response.IsSuccessStatusCode )
            {
                Item.Buyer = buyerId;
                RefreshButton();
                StateHasChanged();
            }
        }
    }
}
