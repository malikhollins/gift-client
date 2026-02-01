using System.ComponentModel.DataAnnotations;

namespace ClientApp.Models
{
    public class House
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The Gifthouse name must be between 2 and 30 characters.")]
        public string? Name { get; set; }

        public string? Email { get; set; }

        public int Owner { get; set; }

        public int OwnerName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Name, Owner, Id);
        }
    }
}
