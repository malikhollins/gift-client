using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class LoadingIndicator
    {
        [Parameter]
        public bool IsLoading { get; set; }

        [Parameter]
        public RenderFragment? CustomLoadingContent { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
