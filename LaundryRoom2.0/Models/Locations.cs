using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryRoom20.Models
{
    public class Locations
    {
        [Key]
        public string Name { get; set; }
        public int Duplicates { get; set; }
    }
}
