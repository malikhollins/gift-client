using ClientApp.Models;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class ItemCard
    {
        [Parameter]
        public int ListId { get; set; }

        [Parameter]
        public Item Model { get; set; } = new();
    }
}
