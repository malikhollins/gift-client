using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class ShowModal
    {
        private Modal modal = default!;

        [Parameter] public RenderFragment? ChildContent { get; set; }

        [Parameter] public EventCallback OnCloseCallback { get; set; }
      
        private Task ShowCreateHomeModalAsync() => modal.ShowAsync<CreateHome>(title: "Create home", parameters: new Dictionary<string, object> 
        {
            {  
                "OnSubmitCompleted" , EventCallback.Factory.Create(this, 
                    async () =>
                    {
                        await modal.HideAsync();
                        await OnCloseCallback.InvokeAsync();
                    }
                    ) 
            }
        });
    }
}
