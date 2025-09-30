namespace Models
{
    public class CreateHouseRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public List<int>? InvitedUsers { get; set; }
    }
}
