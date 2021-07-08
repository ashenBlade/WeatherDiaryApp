using System;
using WeatherDiary.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WeatherDiary.Data;
using WeatherDiary.Server.Infrastructure;
using WeatherDiary.Server.Models;
using User = WeatherDiary.Domain.User;

namespace WeatherDiary.Server.Controllers
{
    [Authorize]
    public class DiaryController : Controller
    {
        private IWeatherDiaryRepository repository;
        public DiaryController(IWeatherDiaryRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Subscribe()
        {
            string email = HttpContext.User.Identity.Name;
            var model = new SubscribeViewModel(email, repository);
            return View(model);
        }

        [HttpPost]
        public IActionResult Subscribe([FromForm] SelectCity city)
        {
            string email = HttpContext.User.Identity.Name;
            string errorMessage = "Выберите город";
            var model = new SubscribeViewModel(email, repository, errorMessage: errorMessage);
            if (isCityNameCorrect(city))
            {
                repository.StartDiary(email, city.Name);
                var successMessage = "Дневник успешно добавлен";
                model = new SubscribeViewModel(email, repository, successMessage: successMessage);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Select()
        {
            string email = HttpContext.User.Identity.Name;
            var model = new SelectDiaryViewModel(email, repository);
            return View(model);
        }

        [HttpPost]
        public IActionResult Select([FromForm] SelectDiaryOptions options)
        {
            if(options.CityName == null)
            {
                string email = HttpContext.User.Identity.Name;
                var errorMessage = "Выберите город";
                var model = new SelectDiaryViewModel(email, repository, errorMessage: errorMessage);
                return View(model);
            }
            return Show(options);
        }

        [HttpGet]
        public IActionResult Unsubscribe()
        {
            string email = HttpContext.User.Identity.Name;
            var model = new UnsubscribeViewModel(email, repository);
            return View(model);
        }

        [HttpPost]
        public IActionResult Unsubscribe([FromForm] SelectCity city)
        {
            string email = HttpContext.User.Identity.Name;
            string errorMessage = "Вы отписались от всех дневников";
            var model = new UnsubscribeViewModel(email, repository, errorMessage: errorMessage);
            if (isCityNameCorrect(city))
            {
                repository.StopDiary(email, city.Name);
                var successMessage = "Дневник успешно остановлен";
                model = new UnsubscribeViewModel(email, repository, successMessage: successMessage);
            }
            return View(model);
        }

        private IActionResult Show(SelectDiaryOptions options)
        {
            var email = HttpContext.User.Identity.Name;
            var records = repository.GetRecords(email, options.CityName);
            records.Sort((r1, r2) => DateTime.Compare(r1.Date, r2.Date));
            var viewModel = new ShowDiaryViewModel() { Options = options, Records = records };
            return View("Show", viewModel);
        }

        private bool isCityNameCorrect(SelectCity city)
        {
            return city != null && city.Name != null;
        }
    }
}
