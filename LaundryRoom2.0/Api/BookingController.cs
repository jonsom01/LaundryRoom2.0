using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaundryRoom20.Models;
using Microsoft.EntityFrameworkCore;
using LaundryRoom20.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<IEnumerable<Booking>> Get()
        {
            return await _repository.GetBookings();
        }

        [HttpGet]
        [Route("checkaddress")]
        public async Task<IActionResult> CheckAddress(string address)
        {
            if (!await _repository.CheckAddress(address))
                return NotFound();
            return Ok();
        }

        [HttpGet]
        [Route("checkpass")]
        public async Task<IActionResult> CheckPass(string address, string pass)
        {
            var bookerId = await _repository.CheckPass(address, pass);
            if (bookerId==null)
                return NotFound();
            return Ok(bookerId);
        }

        // POST api/values
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Booking_Local booking)
        {
            var bookings = await _repository.GetBookings();
            var bookerId = await _repository.CheckPass(booking.BookerAddress, booking.BookerPass);
            if (bookerId == null)
                return NotFound();
            var _booking = new Booking() { BookerId = bookerId, Time = booking.Time };

            var matchingBooking = bookings.Where(b => b.Time == booking.Time).FirstOrDefault();

            if (matchingBooking != null)
            {
                if (matchingBooking.BookerId == _booking.BookerId)
                {
                    if (await _repository.EraseBooking(matchingBooking) >= 0)
                        return NoContent();
                    return StatusCode(500, "Something bad happended");
                }
                return NotFound();
            }
            if (await _repository.UpdateBooking(_booking) >= 0)
                return NoContent();
            return StatusCode(500, "Something bad happended");
        }

    }
}
