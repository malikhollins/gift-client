using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;
using SharedModels;

namespace ClientApp.Components.Extra
{
    public partial class CreateItem
    {
        [Inject] private ListService ListService { get; set; } = null!;
        [CascadingParameter] public EventCallback OnSubmitCompleted { get; set; } = default!;
        [CascadingParameter( Name = "ListId" )] public int ListId { get; set; } = default!;
        public Item ItemData { get; set; } = null!;
        public EditContext? EditContext { get; set; }
        
        protected override void OnInitialized()
        {
            ItemData = new Item();
            EditContext = new EditContext(ItemData);
        }

        public async Task Submit()
        {
            var createItemRequest = new CreateItemRequest
            {
                Name = ItemData.Name,
                Description = ItemData.Description,
                OwnerId = ItemData.OwnerId,
                Link = ItemData.Link,
                Price = ItemData.Price,
                ListId = ListId
            };

            await ListService.CreateItemAsync(createItemRequest);
            await OnSubmitCompleted.InvokeAsync();
        }
    }
}
