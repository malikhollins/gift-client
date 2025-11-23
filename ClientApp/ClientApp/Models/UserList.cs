using System.ComponentModel.DataAnnotations;

namespace ClientApp.Models
{
    public class UserList
    {
        public int Id { get; set; }
        
        public int House { get; set; }
        
        public int Owner { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The Gifthouse name must be between 2 and 30 characters.")]
        public string? Name { get; set; }
    }
}
