using System.ComponentModel.DataAnnotations;

namespace ClientApp.Models
{
    public class GiftHouse
    {
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The Gifthouse name must be between 2 and 30 characters.")]
        public string? HouseName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Your name/alias must be between 2 and 30 characters.")]
        public string? CreatorName { get; set; }
    }
}
