using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryRoom20.Models
{
    public class UserRequestPin
    {
        public string ErrorMessage { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string BookerId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
