using ClientApp.Components.Extra;
using ClientApp.Components.Extra.Forms;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class ListPage
    {
        [Inject] public ListService ListService { get; set; } = default!;
        [Inject] public NavStateService NavStateService { get; set; } = default!;
        [Inject] public UserInfoService UserInfoService { get; set; } = default!;
        [Parameter] public int ListId { get; set; }
        [Parameter] public int HouseId { get; set; }
        [Parameter] public int UserId { get; set; }

        private List<Item> _itemsInList = [];

        private bool _isLoading;

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
        }

        private void OnItemDeleted( int id )
        {
            _itemsInList.RemoveAll( item => item.Id == id );
            StateHasChanged();
        }

        private async Task RefreshItemsInList() => 
           _itemsInList = await ListService.GetItemsAsync(ListId);
    }
}
