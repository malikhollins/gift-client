namespace ServerApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Link { get; set; }
        public bool Favorite { get; set; }
        public int Buyer { get; set; }
    }
}
