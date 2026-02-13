using ClientApp.Components.Extra.Confirmation;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class ListCard
    {
        [Inject] private ListService ListService { get; set; } = null!;
        [Inject] private ListPageObserver ListPageObserver { get; set; } = null!;
        [Parameter] public UserList Model { get; set; } = new();
        [Parameter] public int HouseId { get; set; }
        [Parameter] public bool EditMode { get; set; }

        private CenterModalParameters? _deleteListParameters;

        protected override void OnParametersSet()
        {
            if (EditMode)
            {
                _deleteListParameters =
                    new CenterModalParameters(typeof(BasicConfirm), "Delete List", EventCallback.Factory.Create(this, DeleteListAsync));
            }
            base.OnParametersSet();
        }
        
        private async Task DeleteListAsync()
        {
            var response = await ListService.DeleteListAsync(Model.ListId);
            if (response.IsSuccessStatusCode)
            {
                var update = new UpdateEventListArgs(Model, UpdateEventType.Delete);
                ListPageObserver.NotifyUpdated(update);
            }
        }
    }
}