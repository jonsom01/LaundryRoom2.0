using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryRoom20.Models
{
    public class LaundryRoomContext : IdentityDbContext<ApplicationUser>
    {
        public LaundryRoomContext(DbContextOptions<LaundryRoomContext> options) : base(options)
        {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<Booking> Booking { get; set; }      
        public DbSet<Locations> Locations { get; set; }

    }
}
