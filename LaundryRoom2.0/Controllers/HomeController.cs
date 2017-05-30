using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using LaundryRoom20.Models;
using LaundryRoom20.Api;

namespace LaundryRoom20.Controllers
{
    public class HomeController : Controller
    {
        [Route("/{location}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
