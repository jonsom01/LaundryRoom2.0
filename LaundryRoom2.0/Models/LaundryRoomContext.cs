using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryRoom20.Models
{
    public class LaundryRoomContext : DbContext
    {
        public LaundryRoomContext(DbContextOptions<LaundryRoomContext> options) : base(options)
        {
            
        }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Locations> Locations { get; set; }
    }
}
