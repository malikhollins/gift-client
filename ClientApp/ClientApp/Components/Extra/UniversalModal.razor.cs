using BlazorBootstrap;
using ClientApp.Models;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class UniversalModal
    {
        private Modal? modal;
        private bool showWhenReady;

        [Parameter] public RenderFragment? ChildContent { get; set; }

        [Parameter] public CenterModalParameters? ModalParameters { get; set; }

        public EventCallback OnSubmitCompleted { get; set; }
        public EventCallback OnClose { get; set; }

        public Task ShowModalAsync()
        {
            if (modal != null)
            {
                return modal.ShowAsync();
            }

            showWhenReady = true;
            return Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (showWhenReady && modal != null)
            {
                showWhenReady = false;
                await modal.ShowAsync();
            }
        }

        protected override Task OnParametersSetAsync()
        {
            OnSubmitCompleted = EventCallback.Factory.Create(this, async () => 
            {
                if (modal is null)
                    return;

                await modal.HideAsync();
                if ( ModalParameters is null)
                    return;
                await ModalParameters.OnCloseCallback.InvokeAsync(null);
            });

            OnClose = EventCallback.Factory.Create(this, async () => 
            {
                if (modal is null)
                    return;

                await modal.HideAsync();
            });

            return base.OnParametersSetAsync();
        }
    }
}
