using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;
using SharedModels;

namespace ClientApp.Components.Extra.Forms
{
    public partial class CreateItem
    {
        [Inject] private ListService ListService { get; set; } = null!;
        [Inject] private UserInfoService UserInfoService { get; set; } = null!;
        [CascadingParameter] public EventCallback OnSubmitCompleted { get; set; } = default!;
        [CascadingParameter( Name = "ListId" )] public int ListId { get; set; } = default!;
        public Item ItemData { get; set; } = null!;
        public EditContext? EditContext { get; set; }

        private bool _submitting = false;

        protected override void OnInitialized()
        {
            ItemData = new Item();
            EditContext = new EditContext(ItemData);
        }

        public async Task Submit()
        {
            _submitting = true;

            var id = UserInfoService.GetUserInfo()?.Id ?? 0;
            var createItemRequest = new CreateItemRequest
            {
                Name = ItemData.Name,
                Description = ItemData.Description,
                OwnerId = id,
                Link = ItemData.Link,
                Price = ItemData.Price,
                ListId = ListId
            };

            await ListService.CreateItemAsync(createItemRequest);
            await OnSubmitCompleted.InvokeAsync();

            _submitting = false;
        }
    }
}
