using ClientApp.Components.Extra;
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class HousePage
    {
        [Inject]
        public ListService ListService { get; set; } = null!;

        [Parameter]
        public int HouseId { get; set; }

        private List<UserList> _userLists { get; set; } = new();

        private CenterModalParameters _centerModalParameters { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _userLists = await ListService.GetListsAsync(HouseId);
            _centerModalParameters = new CenterModalParameters(
                typeof(CreateList),
                "Create New List",
                EventCallback.Factory.Create(this, RefreshListsAsync));
        }

        private async Task RefreshListsAsync() =>
            _userLists = await ListService.GetListsAsync(HouseId);
    }
}
