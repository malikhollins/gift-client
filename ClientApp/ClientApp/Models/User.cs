using System.ComponentModel.DataAnnotations;

namespace ClientApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your name to continue.")]
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
