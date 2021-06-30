using Common;
using Database;
using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    public class DiaryController : Controller
    {
        public DiaryController(IWeatherDiaryRepository repository)
        {
            this.repository = repository;
        }

        private IWeatherDiaryRepository repository;

        [HttpGet]
        public IActionResult Subscribe()
        {
            string email = HttpContext.User.Identity.Name;
            var model = new SubscribeViewModel(email, null, repository);
            return View(model);
        }

        [HttpPost]
        public IActionResult Subscribe([FromForm] SelectCity city)
        {
            string email = HttpContext.User.Identity.Name;
            string message = "Выберите город";
            if (isCityNameCorrect(city))
            {
                repository.StartDiary(email, city.Name);
                message = "Дневник успешно добавлен";
            }
            var model = new SubscribeViewModel(email, message, repository);
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
            return RedirectToAction("Show",
                new
                {
                    cityName = options.CityName,
                    temperature = options.Temperature,
                    pressure = options.Pressure,
                    wind = options.Wind,
                    precipitations = options.Precipitations,
                    phenomena = options.Phenomena,
                    cloude = options.Cloudy
                });
        }

        [HttpGet]
        public IActionResult Unsubscribe()
        {
            string email = HttpContext.User.Identity.Name;
            var model = new UnsubscribeViewModel(email, null, repository);
            return View(model);
        }

        [HttpPost]
        public IActionResult Unsubscribe([FromForm] SelectCity city)
        {
            string email = HttpContext.User.Identity.Name;
            string message = "Вы отписались от всех дневников";
            if (isCityNameCorrect(city))
            {
                repository.StopDiary(email, city.Name);
                message = "Дневник успешно остановлен";
            }
            var model = new UnsubscribeViewModel(email, message, repository);
            return View(model);
        }

        [HttpPost]
        public IActionResult Show()
        {
            var model = new ShowDiaryViewModel();
            return View(model);
        }

        [HttpGet]
        public IActionResult Show(string cityName,
            bool temperature,
            bool pressure,
            bool wind,
            bool precipitations,
            bool phenomena,
            bool cloudy)
        {
            var options = new SelectDiaryOptions(cityName,
                temperature,
                pressure,
                wind,
                precipitations,
                phenomena,
                cloudy);
            string email = HttpContext.User.Identity.Name;
            var model = new ShowDiaryViewModel(email, repository, options);
            return View(model);
        }

        private bool isCityNameCorrect(SelectCity city)
        {
            return city != null && city.Name != null;
        }
    }
}