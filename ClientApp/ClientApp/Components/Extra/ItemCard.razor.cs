using ClientApp.Components.Extra.Confirmation;
using ClientApp.Components.Extra.Forms;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class ItemCard
    {
        [Parameter]
        public int ListId { get; set; }

        [Parameter]
        public Item Model { get; set; } = new();

        [Parameter]
        public int OwnerId { get; set; }

        [Parameter]
        public Action<int> OnItemDeleted { get; set; } = null!;

        [Parameter]
        public Action<Item> OnItemEdited { get; set; } = null!;

        [Inject] private UserInfoService UserInfoService { get; set; } = null!;

        [Inject] private ListService ListService { get; set; } = null!;

        private UniversalModal? editModal;
        private UniversalModal? deleteModal;
        private CenterModalParameters? editModalParameters;
        private CenterModalParameters? deleteModalParameters;
        private bool userOwnsItem;
        private Action<bool>? onItemfavorited;

        protected override void OnInitialized()
        {
            var user = UserInfoService.GetUserInfo();
            userOwnsItem = user == null ? false : user.Id == OwnerId;

            OnItemEdited = item =>
            {
                Model = item;
                StateHasChanged();
            };

            onItemfavorited = isFavorited =>
            {
                Model.Favorite = isFavorited;
                StateHasChanged();
            };

            editModalParameters = new CenterModalParameters(
                TypeOfModal: typeof(EditItem),
                Title: "Edit Items",
                OnCloseCallback: EventCallback.Factory.Create(this, EventCallback.Empty));

            deleteModalParameters = new CenterModalParameters(
                TypeOfModal: typeof(BasicConfirm),
                Title: "Delete item?",
                OnCancelCallback: EventCallback.Empty,
                OnCloseCallback: EventCallback.Factory.Create(this, DeleteItemAsync));
        }

        protected async Task ShowEditModalAsync()
        {
            if (editModal != null)
            {
                await editModal.ShowModalAsync();
            }
        }

        protected async Task ShowDeleteModalAsync()
        {

            if (deleteModal != null)
            {
                await deleteModal.ShowModalAsync();
            }
        }

        protected async Task DeleteItemAsync()
        {
            var response = await ListService.DeleteItemAsync(ListId, Model.Id);
            if (response.IsSuccessStatusCode)
            {
                OnItemDeleted?.Invoke(Model.Id);
            }
        }
    }
}
