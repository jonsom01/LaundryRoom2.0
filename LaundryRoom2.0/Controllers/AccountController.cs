using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LaundryRoom20.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LaundryRoom20.Controllers
{
    
    public class AccountController : Controller
    {
        private LaundryRoomContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            LaundryRoomContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (user.Email == null)
            {
                return RedirectToAction("LoginError", "Home", new LoginViewModel { ErrorMessage = "You have to log in as administrator to view this page" });
            }

            var appUser = _context.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (appUser == null)
            {
                user.ErrorMessage = "Login failed - Email address not found";
                return RedirectToAction("LoginError", "Home", user);
            }
            var result = await _signInManager.PasswordSignInAsync(appUser, user.Password, false, false);
            if (result.Succeeded)
            {
                var userLocation = new User { Location = appUser.Location };
                return RedirectToAction("UserCreate", "Home", userLocation);
            }
            else
            {
                user.ErrorMessage = "Login failed - incorrect password";
                return RedirectToAction("LoginError", "Home", user);
            }
        }
    }
}
