using System.ComponentModel.DataAnnotations;
using SharedModels;

namespace ClientApp.Models
{
    public class Item : NotificationObject
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Link { get; set; }
        public bool Favorite { get; set; }
        public int? Buyer { get; set; }
        public string? BuyerName { get; set; }

        private bool _deleted;
        public bool Deleted
        {
            get => _deleted;
            set => SetField(ref _deleted, value);
        }

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
        
        public void Update( Item item )
        {
            Name = item.Name;
            Description = item.Description;
            Buyer = item.Buyer;
            BuyerName = item.BuyerName;
            Favorite = item.Favorite;
            OnPropertyChanged();
        }
        
        public void Update(UpdateItemRequest item)
        {
            Name = item.Name;
            Description = item.Description;
            Price = item.Price;
            Link = item.Link;
            OnPropertyChanged();
        }

        public void Update(UpdateBuyerInItemRequest buyerInItemRequest)
        {
            Buyer = buyerInItemRequest.BuyerId;
            BuyerName = string.Empty; // TODO: fill this in
            OnPropertyChanged();
        }

        public void Update(UpdateFavoriteItemRequest favoriteItemRequest)
        {
            Favorite = favoriteItemRequest.Favorited;
            OnPropertyChanged();
        }
    }
}
