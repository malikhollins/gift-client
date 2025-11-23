using Microsoft.AspNetCore.Components;

namespace ClientApp.Models
{
    public record CenterModalParameters( Type TypeOfModal, string Title, EventCallback OnCloseCallback );
}
