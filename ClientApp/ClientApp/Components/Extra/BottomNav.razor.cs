using ClientApp.Models;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class BottomNav
    {
        [Parameter] public string? GoBackUrl { get; set; }
        [Parameter] public CenterModalParameters CenterModalParameters { get; set; } = default!;
    }
}