using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using SharedModels;

namespace ClientApp.Components.Extra
{
    public partial class ItemButtons
    {
        [Inject] private ListService ListService { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;
        [Parameter] public Item ItemData { get; set; } = null!;
        [Parameter] public int ListId { get; set; }

        private CenterModalParameters? ModalParameters { get; set; }

        private enum EditType
        {
            Undefined,
            Favorite,
            UpdateBuyer,
            ModifyItem
        }

        protected override void OnInitialized()
        {
            ModalParameters = new CenterModalParameters(
                TypeOfModal: typeof(EditItem), 
                Title: "Edit Items", 
                OnCloseCallback: EventCallback.Empty);
        }

        private async Task EditItemType(EditType type)
        {
            var user = UserInfoService.GetUserInfo();
            if (user == null)
            {
                return;
            }

            switch (type)
            {
                case EditType.Favorite:
                    var request = new UpdateFavoriteItem
                    {
                        ItemId = ItemData.Id,
                        ListId = ListId,
                        UserId = user.Id,
                        Favorited = !ItemData.Favorited
                    };
                    await ListService.UpdateFavoriteInList(request);
                    ItemData.Favorited = !ItemData.Favorited;
                    break;
                case EditType.UpdateBuyer:
                    var updateBuyerRequest = new UpdateBuyerInListRequest
                    {
                        ItemId = ItemData.Id,
                        ListId = ListId,
                        BuyerId = user.Id
                    };
                    await ListService.UpdateBuyerAsync(updateBuyerRequest);
                    ItemData.Buyer = user.Id;
                    break;
                case EditType.ModifyItem:
                    // Implementation for modifying the item with a modal goes here
                    break;
                default:
                    break;
            }
        }
    }
}
