using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            const string errorMessage = "Неправильные почта и (или) пароль";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", errorMessage);
                return View(viewModel);
            }

            var userRegistered = _repository.ContainsUser(viewModel.Email);

            if (!userRegistered)
            {
                ModelState.AddModelError("", errorMessage);
                return View(viewModel);
            }

            var dbUser = _repository.GetUser(viewModel.Email, viewModel.Password);
            if (dbUser is null)
            {
                ModelState.AddModelError("", errorMessage);
                return View(viewModel);
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
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Неправильные почта и (или) пароль");
                return View(viewModel);
            }

            var userAlreadyRegistered = _repository.ContainsUser(viewModel.Email);
            if (userAlreadyRegistered)
            {
                ModelState.AddModelError("", "Пользователь с такой почтой уже зарегистрирован");
                return View(viewModel);
            }

            var user = _repository.AddUser(viewModel.Email, viewModel.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Извините. Что-то пошло не так");
                return View(viewModel);
            }

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