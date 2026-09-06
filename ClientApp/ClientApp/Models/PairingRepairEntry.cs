namespace ClientApp.Models
{
    public sealed class PairingRepairEntry
    {
        public int UserId { get; set; }
        public int ListId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
