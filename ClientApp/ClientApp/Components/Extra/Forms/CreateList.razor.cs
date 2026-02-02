using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedModels;
using System.Diagnostics;

namespace ClientApp.Components.Extra.Forms
{
    public partial class CreateList
    {
        [Inject] ListService ListService { get; set; } = null!;
        [Inject] UserInfoService UserInfoService { get; set; } = null!;
        [CascadingParameter] public EventCallback OnSubmitCompleted { get; set; } = default!;
        [CascadingParameter( Name = "HouseId")] public int HouseId { get; set; } = default!;

        public UserList ListData { get; set; } = null!;
        public EditContext? EditContext { get; set; }

        private bool _submitting = false;

        protected override void OnInitialized()
        {
            ListData = new UserList();
            EditContext = new EditContext(ListData);
        }

        public async Task Submit()
        {
            _submitting = true;

            var request = new CreateListRequest();
            request.Name = ListData.Name;
            request.OwnerId = UserInfoService.GetUserInfo()?.Id ?? -1;
            request.HouseId = HouseId;

            try
            {
                await ListService.CreateListAsync(request);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            await OnSubmitCompleted.InvokeAsync();

            _submitting = false;
        }
    }
}
