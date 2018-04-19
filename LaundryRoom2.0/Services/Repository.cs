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

        public async Task<User> GetUserAsync(string bookerId)
        {
            return await _context.User.Where(u => u.BookerId == bookerId).FirstOrDefaultAsync();
        }

        public async Task<User> ConfirmEmail(string code)
        {
            var user = await _context.User.Where(u => u.MailConfirmationCode == code).FirstOrDefaultAsync();
            if (user != null)
            {
                user.EmailConfirmed = true;
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<string> CreateAndSaveNewPin(User user)
        {
            var newPin = CreatePass();
            var newSalt = CreateSalt(20);
            user.Password = CreateHash(newPin, newSalt);
            user.Salt = newSalt;
            _context.Update(user);
            try
            {
                await _context.SaveChangesAsync();
                return newPin;
            }
            catch (Exception e)
            {
                var errMess = e.InnerException.Message;
                return null;
            }
        }

        public async Task<string> SaveEmailConfirmationCode(string code, User user)
        {
            var error = "";
            user.MailConfirmationCode = code;
            user.EmailConfirmed = false;
            _context.Update(user);

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (Exception e)
            {
                error = e.InnerException.Message;
            }
            return error;
        }

        public async Task<string> SaveEmailAddress(User user)
        {
            var errorMessage = "";
            var dbUser = await _context.User.FirstOrDefaultAsync(u => u.BookerId == user.BookerId);
            dbUser.Email = user.Email;
            _context.Update(dbUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                errorMessage = e.InnerException.Message;
            }
            return errorMessage;
        }

        public async Task<bool> CheckBookerId(User user)
        {
            return await _context.User.AnyAsync(u => u.BookerId == user.BookerId);
        }

        public async Task<bool> CheckBookerId(UserRequestPin user)
        {
            return await _context.User.AnyAsync(u => u.BookerId == user.BookerId);
        }

        public async Task<bool> EmailRegistered(User user)
        {
            var dbUser = await _context.User.FirstOrDefaultAsync(u => u.BookerId == user.BookerId);
            var returnBool = (!String.IsNullOrEmpty(dbUser.Email) && user.Email == dbUser.Email);
            return returnBool;
        }

        public async Task<bool> EmailRegistered(UserRequestPin user)
        {
            var dbUser = await _context.User.FirstOrDefaultAsync(u => u.BookerId == user.BookerId);
            var returnBool = (!String.IsNullOrEmpty(dbUser.Email) && user.Email == dbUser.Email);
            return returnBool;
        }

        public async Task<bool> PasswordOk(User user)
        {
            return await _context.User.AnyAsync(u => u.Password == CreateHash(user.Password, u.Salt));
        }

        public async Task<ApplicationUser> GetApplicationUser(string email)
        {
            return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<string> CreateUser(User user, String pass)
            {
            var errorMessage = "";
            if (await CheckBookerId(user))
            {
                return "There already is a user with this BookerId";
            }
            var booking = new Booking();
            user.Booking = booking;
            booking.User = user;
            user.Salt = CreateSalt(20);
            user.Password = CreateHash(pass, user.Salt);
            
            await _context.AddAsync(user);
            await _context.AddAsync(booking);
            try
            {
                await _context.SaveChangesAsync();
            }

            catch (Exception e)
            {
                errorMessage = e.InnerException.Message;
            }

            return errorMessage;
        }

        public async Task<IEnumerable<Booking>>GetBookings(string location)
        {
            return await _context.Booking.Where(b => b.Time != null && b.User.Location == location).ToListAsync();
        }

        public bool CheckLocation(string location)
        {
            return _context.Locations.Where(l => l.Name == location).Any();
        }

        public int NrOfDuplicates(string location)
        {
            return _context.Locations.Where(l => l.Name == location).FirstOrDefault().Duplicates;
        }

        public async Task<User> CheckPass(Booking b)
        {      
            var user = await _context.User.Where(u => u.Password.Equals(CreateHash(b.User.Password, u.Salt)) &&
            u.Location.Equals(b.User.Location)).FirstOrDefaultAsync();
            return user;
        }

        public async Task<int> EraseBooking(Booking booking)
        {
            booking.Time = null;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateBooking(Booking booking)
        {
            var _booking = await _context.Booking.Where
                (b => b.User.Id == booking.User.Id)
                .FirstOrDefaultAsync();
            _booking.Time = booking.Time;
            _booking.BookerId = booking.User.BookerId;

            return await _context.SaveChangesAsync();
        }

        public string CreatePass()
        {
            Random RandomPIN = new Random();
            string RandomPINResult = "";
            for (int i = 0; i < 4; i++)
            {
                RandomPINResult += RandomPIN.Next(1, 9).ToString();
            }
            return RandomPINResult;

        }

        public string CreateSalt(int size)
        {
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var buff = new byte[size];
            rng.GetBytes(buff);
            var returnString = Convert.ToBase64String(buff);
            return returnString.Replace('+', '_');

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
