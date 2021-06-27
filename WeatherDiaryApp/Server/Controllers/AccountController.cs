using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Неправильные логин и (или) пароль");
                return View(model);
            }

            // Logic for checking whether user registered

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Неправильные почта и (или) пароль");
                return View(model);
            }

            // Checking whether user already registered
            // Registering if not -> Redirect to Login
            // Error - otherwise
            return RedirectToAction("Login");
        }
    }
}