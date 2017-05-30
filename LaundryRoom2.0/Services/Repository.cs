using LaundryRoom20.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryRoom20.Services
{
    public class Repository
    {
        private LaundryRoomContext _context;
        private DateTime date = DateTime.Now;
        public Repository(LaundryRoomContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>>GetBookings(string location)
        {
            return await _context.Booking.Where(b => b.Time != null && b.User.Location == location).ToListAsync();
        }

        public async Task<string> CheckPass(Booking b)
        {
            var user = await _context.User.Where(u => u.Password.Equals(CreateHash(b.User.Password, u.Salt)) &&
            u.Location.Equals(b.User.Location)).FirstOrDefaultAsync();
            return user.BookerId;
        }

        private async Task <int> EraseBookings(string date)
        {
            var bookings = await _context.Booking.Where(b => b.Time.Contains(date)).ToListAsync();
            foreach (var booking in bookings)
            {
                booking.Time = null;
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> EraseBooking(Booking booking)
        {
            booking.Time = null;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateBooking(Booking booking)
        {
            var _booking = await _context.Booking.Where
                (b => b.BookerId == booking.BookerId)
                .FirstOrDefaultAsync();

            _booking.Time = booking.Time;

            return await _context.SaveChangesAsync();
        }

        private string CreateHash(string input, string salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            var sha256HashString = System.Security.Cryptography.SHA256.Create();
            byte[] hash = sha256HashString.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

    }
}
