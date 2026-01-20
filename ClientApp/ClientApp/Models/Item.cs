namespace ClientApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Link { get; set; }
        public bool Favorite { get; set; }
        public int Buyer { get; set; }
        public string? BuyerName { get; set; }

        public Item()
        {
        }

        public Item( Item item )
        {
            Id = item.Id;
            ListId = item.ListId;
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Link = item.Link;
            Favorite = item.Favorite;
            Buyer = item.Buyer;
            BuyerName = item.BuyerName;
        }
    }
}
