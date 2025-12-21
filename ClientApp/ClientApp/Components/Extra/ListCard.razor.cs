using ClientApp.Models;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra
{
    public partial class ListCard
    {
        [Parameter]
        public UserList Model { get; set; } = new();
        [Parameter]
        public int HouseId { get; set; }
    }
}
