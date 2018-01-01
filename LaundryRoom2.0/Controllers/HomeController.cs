using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using LaundryRoom20.Models;
using LaundryRoom20.Api;
using LaundryRoom20.Services;

namespace LaundryRoom20.Controllers
{
    public class HomeController : Controller
    {
        private Repository _repository;
        private LaundryRoomContext _context;

        public HomeController(Repository repository, LaundryRoomContext context)
        {
            _context = context;
            _repository = repository;
        }

        [Route("/{location}")]
        public IActionResult Index(string location)

        {
            if (_repository.CheckLocation(location))
                return View();
            else
                return StatusCode(404, "No Content");
        }

        [Route("/{location}/1")]
        public IActionResult Index1(string location)
      
        {
            if (_repository.CheckLocation(location) && _repository.NrOfDuplicates(location) >= 1)
                return View("Index");
            else
                return StatusCode(404, "No Content");
        }

        [Route("/{location}/2")]
        public IActionResult Index2(string location)

        {
            if (_repository.CheckLocation(location) && _repository.NrOfDuplicates(location) >= 2)
                return View("Index2");
            else
                return StatusCode(404, "No Content");
        }
    }
}
