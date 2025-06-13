using WebAPI.Models;

namespace ServerApp.Models
{
    public class UserInvites
    {
        public House? House { get; init; }
        public InviteStatus InviteStatus { get; init; }
    }
}
