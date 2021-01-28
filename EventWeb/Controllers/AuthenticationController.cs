using EventWeb.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        public AuthenticationController(IAuthenticationService authenticationService,IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }
        public IActionResult Login()
        {
            if (TempData["StatusMessage"] != null)
            {
                ModelState.AddModelError("", TempData["StatusMessage"].ToString());
            }
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM vm)
        {
            //vm.Password = PasswordHelper.Sha256(vm.Password, vm.Username);
            var user = _authenticationService.Login(vm.Username, vm.Password, vm.RememberMe);            
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return RedirectToAction("SendConfirmMail", "User", new { userId = user.Id, email = user.Email });
                }
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Password", "Invalid Username/Password");
            return View(vm);
        }
        public IActionResult UserProfile()
        {
            return View();
        }

        public IActionResult Logout()
        {
            _authenticationService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
