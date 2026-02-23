using ClientApp.Components.Extra;
using ClientApp.Components.Extra.Forms;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SharedModels;

namespace ClientApp.Components.Pages
{
    public partial class ListPage : IAsyncDisposable
    {
        [Inject] public ListService ListService { get; set; } = default!;
        [Inject] public NavStateService NavStateService { get; set; } = default!;
        [Inject] public UserInfoService UserInfoService { get; set; } = default!;
        [Inject] public HubService HubService { get; set; } = default!;
        [Parameter] public int ListId { get; set; }
        [Parameter] public int HouseId { get; set; }
        [Parameter] public int UserId { get; set; }
        [Parameter] public string OwnerName { get; set; } = null!;
        [Parameter] public string ListName { get; set; } = null!;

        private List<Item> _itemsInList = [];
        private bool _isLoading;
        private HubConnection? Connection { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            await base.OnInitializedAsync();

            await RefreshItemsInList();

            CenterModalParameters? param = null;
            if (UserInfoService.GetUserInfo()?.Id == UserId)
            {
                param = new CenterModalParameters
                (
                    TypeOfModal: typeof(CreateItem),
                    Title: "Create Item",
                    OnCloseCallback: EventCallback.Factory.Create(this, RefreshItemsInList)
                );
            }

            NavStateService.UpdateNavState(new NavState
            {
                GoBackUrl = $"/homepage",
                CenterModalParameters = param,
                HouseId = HouseId,
                ListId = ListId,
            });

            _isLoading = false;

            RegisterListConnectionAsync().SafeFireAndForget();
        }

        private async Task RegisterListConnectionAsync()
        {
            var user = UserInfoService.GetUserInfo();
            if (user == null)
            {
                return;
            }

            Connection ??= HubService.Build();
            await Connection.SendAsync("JoinList", ListId);

            Connection.On("NotifyItemCreated", (object obj) =>
            {
                if (obj is Item item)
                {
                    var inList = _itemsInList.Any(i => i.Id == item.Id);
                    if (!inList)
                    {
                        _itemsInList.Add(item);
                    }
                }
            });

            Connection.On("NotifyItemUpdated", (object obj) =>
            {
                if (obj is UpdateItemRequest item)
                {
                    var itemInList = _itemsInList.FirstOrDefault(i => i.Id == item.ItemId);
                    itemInList?.Update(item);
                }
            });

            Connection.On("NotifyItemDeleted", (object obj) =>
            {
                if (obj is int itemId)
                {
                    var itemInList = _itemsInList.FirstOrDefault(i => i.Id == itemId);
                    if (itemInList != null)
                    {
                        itemInList.Deleted = true;
                    }
                }
            });

            Connection.On("NotifyFavoriteUpdated", (object obj) =>
            {
                if (obj is UpdateFavoriteItemRequest favoriteItemRequest)
                {
                    var itemInList = _itemsInList.FirstOrDefault(i => i.Id == favoriteItemRequest.ItemId);
                    itemInList?.Update(favoriteItemRequest);
                }
            });

            Connection.On("NotifyBuyerUpdated", (object obj) =>
            {
                if (obj is UpdateBuyerInItemRequest buyerInItemRequest)
                {
                    var itemInList = _itemsInList.FirstOrDefault(i => i.Id == buyerInItemRequest.ItemId);
                    itemInList?.Update(buyerInItemRequest);
                }
            });
        }

        private void UpdateItemInList(Item item)
        {
            var itemInList = _itemsInList.FirstOrDefault(i => i.Id == item.Id);
            itemInList?.Update(item);
        }

        private void OnItemDeleted(int id)
        {
            _itemsInList.RemoveAll(item => item.Id == id);
            StateHasChanged();
        }

        private async Task RefreshItemsInList() =>
            _itemsInList = await ListService.GetItemsAsync(ListId);

        public async ValueTask DisposeAsync()
        {
            if (Connection != null) await Connection.DisposeAsync();
        }
    }
}