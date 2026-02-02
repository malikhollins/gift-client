using ClientApp.Models;

namespace ClientApp.Utils
{
    public static class StatusExtentions
    {
        public static string ToDisplayString(this InviteStatus status)
        {
            return status switch
            {
                InviteStatus.Pending => "[Pending]",
                InviteStatus.Accepted => "[Accepted]",
                InviteStatus.Rejected => "[Rejected]",
                _ => "Unknown"
            };
        }
    }
}
