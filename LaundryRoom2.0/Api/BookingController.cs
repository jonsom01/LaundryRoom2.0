using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaundryRoom20.Models;
using Microsoft.EntityFrameworkCore;
using LaundryRoom20.Services;

namespace LaundryRoom20.Api
{

    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private Repository _repository;
        private LaundryRoomContext _context;

        public BookingController(Repository repository, LaundryRoomContext context)
        {
            _context = context;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Booking>> Get(string location)
        {
            return await _repository.GetBookings(location);
        }

        // POST api/values
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Booking booking)
        {
            var bookings = await _repository.GetBookings(booking.User.Location);
            booking.BookerId = await _repository.CheckPass(booking);
            if (booking.BookerId == null)
                return NotFound();

            var matchingBooking = bookings.Where(b => b.Time == booking.Time).FirstOrDefault();

            if (matchingBooking != null)
            {
                if (matchingBooking.BookerId == booking.BookerId)
                {
                    if (await _repository.EraseBooking(matchingBooking) >= 0)
                        return NoContent();
                    return StatusCode(500, "Something bad happended");
                }
                return NotFound();
            }
            if (await _repository.UpdateBooking(booking) >= 0)
                return NoContent();
            return StatusCode(500, "Something bad happended");
        }

    }
}
