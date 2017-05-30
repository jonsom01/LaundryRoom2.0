using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryRoom20.Models
{
    public class User
    {
        [Key]
        public string BookerId { get; set; }
        public Booking Booking { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        [MaxLength(10)]
        public string ShortAddress { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Location { get; set; }
    }
}
