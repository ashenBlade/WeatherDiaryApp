using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using User = Common.User;

namespace Server.Controllers
{
    public class AccountController : Controller
    {
        private IWeatherDiaryRepository _repository;
        public AccountController(IWeatherDiaryRepository repository)
        {
            _repository = repository ?? throw new NullReferenceException(nameof(repository));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Неправильные почта и (или) пароль");
                return View(model);
            }

            if (!_repository.ContainsUser(model.Email))
            {
                ModelState.AddModelError("", "Пользователя с такой почтой не существует");
                return View(model);
            }

            var dbUser = _repository.GetUser(model.Email, model.Password);
            if (dbUser is null)
            {
                ModelState.AddModelError("", "Неправильные почта и (или) пароль");
                return View(model);
            }

            var user = new User() { Email = dbUser.Email, Password = dbUser.Password };

            await Authenticate(HttpContext, user);

            return RedirectToAction("Get", "Diary");
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

            // TODO

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private static async Task Authenticate(HttpContext context, User user)
        {
            var claims = new List<Claim>() { new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email) };
            var id = new ClaimsIdentity(claims, "ApplicationCookie",
                                        ClaimsIdentity.DefaultNameClaimType,
                                        ClaimsIdentity.DefaultRoleClaimType);

            var principal = new ClaimsPrincipal(id);
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}