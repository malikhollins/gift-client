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

        private bool isBought;

        private bool canInteract;

        protected override void OnParametersSet() => RefreshButton();

        private void RefreshButton()
        {
            var user = UserInfoService.GetUserInfo();
            isBought = Item.Buyer == user!.Id;
            canInteract = !isBought || Item.Buyer == null;
        }

        private async Task MarkAsBuyerAsync()
        {
            var user = UserInfoService.GetUserInfo();
            if (user == null)
            {
                return;
            }

            var updateBuyerRequest = new UpdateBuyerInItemRequest
            {
                ItemId = Item.Id,
                ListId = ListId,
                BuyerId = user.Id == Item.Buyer ? null : user.Id
            };

            var response = await ListService.UpdateBuyerAsync(updateBuyerRequest);
            if ( response.IsSuccessStatusCode )
            {
                Item.Buyer = user.Id;
                RefreshButton();
                StateHasChanged();
            }
        }
    }
}
