using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ClientApp.Models
{
    public class UserList : NotificationObject
    {
        public int ListId { get; set; }
        
        public int House { get; set; }
        
        public int Owner { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The name must be between 2 and 30 characters.")]
        public string? Name { get; set; }

        public string? OwnerName { get; set; }

        private bool _deleted;
        public bool Deleted
        {
            get => _deleted;
            set => SetField( ref _deleted, value);
        }
    }
}
