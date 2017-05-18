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

        public async Task<IEnumerable<Booking>>GetBookings()
        {
            if (date.Hour == 12 && date.Minute == 06)
            {
                var yesterday = date.AddDays(-1);
                await EraseBookings(yesterday.Date.Day.ToString());
            }
            return await _context.Booking.Where(b => b.Time != null).ToListAsync();
        }

        public async Task<bool> CheckAddress(string address)
        {
            return await _context.User.AnyAsync
                (u => u.ShortAddress == address);
        }

        public async Task<string> CheckPass(string address, string pass)
        {
            var user = await _context.User.Where(u => u.ShortAddress == address).FirstOrDefaultAsync();
            if (!user.Password.Equals(CreateHash(pass, user.Salt)))
                return null;
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

        private string CreateSalt(int size)
        {
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);

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
