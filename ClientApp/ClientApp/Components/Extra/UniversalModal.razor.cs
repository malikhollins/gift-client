using BlazorBootstrap;
using ClientApp.Models;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class UniversalModal
    {
        private Modal modal = default!;

        [Parameter] public RenderFragment? ChildContent { get; set; }

        [Parameter] public CenterModalParameters? ModalParameters { get; set; }

        public EventCallback OnSubmitCompleted { get; set; }

        private Task ShowModalAsync() => modal.ShowAsync();

        protected override Task OnParametersSetAsync()
        {
            OnSubmitCompleted = EventCallback.Factory.Create(this, async () => 
            {
                await modal.HideAsync();
                if ( ModalParameters is null)
                    return;
                await ModalParameters.OnCloseCallback.InvokeAsync(null);
            });

            return base.OnParametersSetAsync();
        }
    }
}
