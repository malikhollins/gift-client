using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class BottomNav
    {
        [Parameter] public bool CanGoBack { get; set; } = false;

        [Parameter] public EventCallback OnCreateClosed { get; set; }
    }
}
