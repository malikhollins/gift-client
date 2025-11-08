using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class ShowModal
    {
        private Modal modal = default!;

        [Parameter] public RenderFragment? ChildContent { get; set; }

        private Task ShowCreateHomeModalAsync() => modal.ShowAsync<CreateHome>(title: "Create home");
    }
}
