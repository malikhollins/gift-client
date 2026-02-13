using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedModels;

namespace ClientApp.Components.Extra.Forms
{
    partial class EditItem
    {
        [Inject] private ListService ListService { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;
        [CascadingParameter] public EventCallback OnSubmitCompleted { get; set; } = default!;
        [CascadingParameter(Name = "ListId")] public int ListId { get; set; } = default!;
        [CascadingParameter] public Item Item { get; set; } = default!;
        [CascadingParameter] public Action<Item> OnItemEdited { get; set; } = null!;

        public Item ItemData { get; set; } = null!;
        public EditContext? EditContext { get; set; }

        private bool _submitting = false;

        protected override void OnInitialized()
        {
            ItemData = new Item( Item );
            EditContext = new EditContext(ItemData);
        }

        public async Task Submit()
        {
            _submitting = true;

            var id = UserInfoService.GetUserInfo()?.Id ?? 0;
            var updateItemRequest = new UpdateItemRequest
            {
                UserId = id,
                ItemId = ItemData.Id,
                Name = ItemData?.Name ?? string.Empty,
                Description = ItemData?.Description ?? string.Empty,
                Link = ItemData?.Link ?? string.Empty,
                Price = ItemData?.Price ?? default,
                ListId = ListId
            };

            var response = await ListService.UpdateItemAsync(updateItemRequest);
            if (response.IsSuccessStatusCode)
            {
                OnItemEdited.Invoke(ItemData!);
            }

            await OnSubmitCompleted.InvokeAsync();
            _submitting = false;
        }
    }
}
