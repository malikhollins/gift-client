using System.ComponentModel.DataAnnotations;

namespace ClientApp.Models
{
   public class GiftHouse
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The Gifthouse name must be between 2 and 50 characters.")]
        public string? HouseName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Your name/alias must be between 2 and 50 characters.")]
        public string? CreatorName { get; set; }
    }
}
