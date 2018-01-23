using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using LaundryRoom20.Models;
using LaundryRoom20.Api;
using LaundryRoom20.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace LaundryRoom20.Controllers
{
    public class HomeController : Controller
    {
        private Repository _repository;
        private LaundryRoomContext _context;
        private string apiKey;

        public HomeController(Repository repository, LaundryRoomContext context)
        {
            _context = context;
            _repository = repository;
            apiKey = "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";

        }


        public IActionResult Index()
        {
            return View();
        }
        [Route("/registeremail")]
        public IActionResult RegisterEmail()
        {
            return View();
        }
        [Route("/requestnewpin")]
        public IActionResult RequestNewPin()
        {
            return View();
        }

        public IActionResult NewPincodeDelivered()
        {
            return View();
        }

        public async Task<IActionResult> NewPin(UserRequestPin userRequestPin)
        {
            if (userRequestPin == null)
            {
                return View("Error");
            }

            if (!ModelState.IsValid)
            {
                userRequestPin.ErrorMessage = "There was something wrong with your input";
                return View("RequestNewPin", userRequestPin);
            }

            if (!await _repository.CheckBookerId(userRequestPin))
            {
                userRequestPin.ErrorMessage = "User not found";
                return View("RequestNewPin", userRequestPin);
            }

            var dbUser = await _repository.GetUserAsync(userRequestPin.BookerId);

            if (!dbUser.EmailConfirmed)
            {
                userRequestPin.ErrorMessage = "You have to register and confirm your Email-address";
                return View("RequestNewPin", userRequestPin);
            }

            if (await _repository.EmailRegistered(userRequestPin))
                {
                            
                        var newPin = await _repository.CreateAndSaveNewPin(dbUser);
                        var client = new SendGridClient(apiKey);
                        var msg = new SendGridMessage()
                        {
                            From = new EmailAddress("jonsom01@hotmail.com", "Bokatvattstugan.se"),
                            Subject = "Din nya pin-kod",
                            HtmlContent = "<strong> Din nya pin-kod är: " + newPin
                        };
                        msg.AddTo(new EmailAddress(dbUser.Email, "Test User"));
                        var response = await client.SendEmailAsync(msg);
                        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                        {
                            return RedirectToAction("NewPincodeDelivered");
                        }
                       userRequestPin.ErrorMessage = "Email couldn't be delivered";
                       return View("RequestNewPin", userRequestPin);
                    }
            userRequestPin.ErrorMessage = "You have to register and confirm your Email-address";
            return View("RequestNewPin", userRequestPin);
        }

        public async Task<IActionResult> ConfirmEmail(string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            else if (await _repository.ConfirmEmail(code) != null)
            {
                return View("EmailConfirmationSuccessful");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> SendConfirmationEmail(User user)
        {
            if (user == null)
                return View("Error");
            var emailCode = _repository.CreateSalt(20);
            var error = await _repository.SaveEmailConfirmationCode(emailCode, user);
            var link = "http://localhost:50583/home/confirmEmail?code=" + emailCode;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("jonsom01@hotmail.com", "Bokatvattstugan.se"),
                Subject = "E-postregistrering",
                HtmlContent = "<strong>Tack för att du har registrerat din E-postadress hos bokatvattstugan.online! </strong><br/> <h1>Detta är en rubrik</h1>" +
                              "<p>Tryck på <a href=" + link + "> länken </a>för att bekräfta adressen:"
            };
            msg.AddTo(new EmailAddress(user.Email, "Test User"));
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return RedirectToAction("EmailRegistrationSuccessful");
            }
            else
                return View("Error");
        }

        public async Task<IActionResult> EmailRegistered(UserForEmailRegistration userToReturn)
        {
            var errMess = new ErrorMessage("");
            if (!ModelState.IsValid)
            {
                errMess.ErrMess = "There was something wrong with your input.";
                return View("Error", errMess);
            }

            var user = new User();
            user = Mapper.Map<User>(userToReturn);
            if (!await _repository.CheckBookerId(user))
            {
                userToReturn.ErrorMessage = "User not found";
                return View("RegisterEmail", userToReturn);
            }

            if (await _repository.EmailRegistered(user))
            {
                userToReturn.ErrorMessage = "Your Email has already been registered!";
                return View("RegisterEmail", userToReturn);
            }

            if (!await _repository.PasswordOk(user))
            {
                userToReturn.ErrorMessage = "Password incorrect";
                return View("RegisterEmail", userToReturn);
            }
            else
            {
                var error = await _repository.SaveEmailAddress(user);
                if (error == "")
                {
                    return RedirectToAction("SendConfirmationEmail", await _repository.GetUserAsync(user.BookerId));
                }
                else
                {
                    userToReturn.ErrorMessage = error;
                    return RedirectToAction("RegisterEmail", userToReturn);
                }
            }
        }

        public IActionResult CheckLogin([Bind("Email", "Password")] LoginViewModel user)
        {
            if (!ModelState.IsValid || user == null)
            {
                return View("Error");
            }
            return RedirectToAction("Login", "Account", user);
        }

        public IActionResult EmailRegistrationSuccessful()
        {
            return View();
        }

        [Authorize]
        public IActionResult UserCreate(User user)
        {
            if (user == null)
            {
                return View("Error");
            }
            var userLocation = Mapper.Map<UserToRegister>(user);
            return View(userLocation);
        }

        [Route("/adminlogin")]
        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult LoginError(LoginViewModel user)
        {
            return View("AdminLogin", user);
        }


        [Route("/redirecttoroom")]
        public IActionResult RedirectToRoom()
        {
            var locations = new Helper();

            locations.Locations = _context.Locations.Select(l => l.Name).Distinct().ToList();
            return View(locations);
        }

        public IActionResult Redirect([Bind("Location")] Helper helper)
        {
            var loc = "/" + helper.Location;
            return Redirect(loc);
        }

        public async Task<IActionResult> Create([Bind("Name,Address,Email,BookerId,Location")] UserToRegister userToRegister)
        {
            var pass = _repository.CreatePass();
            var user = Mapper.Map<User>(userToRegister);
            var mess = await _repository.CreateUser(user, pass);
            if (mess == "")
            {
                user.Password = pass;
                return View("UserCreated", user);
            }
            else
            {
                userToRegister.ErrorMessage = String.Format("The operation failed - {0}!", mess);
                return View("UserCreate", userToRegister);
            }
     
        }

        [Route("/{location}")]
        public IActionResult Index(string location)

        {
            if (_repository.CheckLocation(location))
                return View("Room");
            else
                return StatusCode(404, "No Content");
        }

        [Route("/{location}/1")]
        public IActionResult Index1(string location)
      
        {
            if (_repository.CheckLocation(location) && _repository.NrOfDuplicates(location) >= 1)
                return View("Room");
            else
                return StatusCode(404, "No Content");
        }

        [Route("/{location}/2")]
        public IActionResult Index2(string location)

        {
            if (_repository.CheckLocation(location) && _repository.NrOfDuplicates(location) >= 2)
                return View("Room2");
            else
                return StatusCode(404, "No Content");
        }
    }
}
