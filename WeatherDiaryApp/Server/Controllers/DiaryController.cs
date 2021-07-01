using System;
using Common;
using Database;
using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure;
using Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
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
            return Show(options);
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

        private IActionResult Show(SelectDiaryOptions options)
        {
            var email = HttpContext.User.Identity.Name;
            var date = new DateTime(2021, 6, 15);
            var records = repository.GetRecords(email, options.CityName, date);
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