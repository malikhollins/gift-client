using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra.Confirmation
{
    partial class BasicConfirm
    {
        [CascadingParameter( Name = "OnClose" )] public EventCallback OnClose { get; set; } = default!;
        [CascadingParameter] public EventCallback OnSubmitCompleted { get; set; } = default!;
    }
}
