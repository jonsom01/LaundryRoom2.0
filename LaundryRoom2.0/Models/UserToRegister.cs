using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryRoom20.Models
{
    public class UserToRegister
    {
        public string ErrorMessage { get; set; }

        [Key]
        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string BookerId { get; set; }
        public Booking Booking { get; set; }
        [MaxLength(50)]
        [Required]
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public string Salt { get; set; }
        [Required]
        public string Location { get; set; }

        public UserToRegister()
        {
            ErrorMessage = "";
        }
    }
}
