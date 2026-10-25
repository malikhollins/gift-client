namespace ClientApp.Models
{
    public enum NavPage
    {
        Other,
        Home,
        Profile
    }

    public class NavState
    {
        public CenterModalParameters? CenterModalParameters { get; set; }

        public int HouseId { get; set; }

        public House? House { get; set; }

        public int ListId { get; set; }

        public NavPage CurrentPage { get; set; } = NavPage.Other;
    }
}
