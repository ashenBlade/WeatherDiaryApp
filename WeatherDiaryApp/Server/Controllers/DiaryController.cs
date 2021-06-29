using Common;
using Database;
using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure;
using Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace Server.Controllers
{
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
            if (city.Name != "Выберите город")
                repository.StartDiary(email, city.Name);
            var model = new SubscribeViewModel(email, "Дневник успешно добавлен", repository);
            return View(model);
        }

        [HttpGet]
        public IActionResult Select()
        {
            var model = new SelectDiaryViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Select([FromForm] SelectDiaryOptions options)
        {
            var model = new SelectDiaryViewModel();
            return View(model);
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
            if (city.Name != "Выберите город")
                repository.StopDiary(email, city.Name);
            var model = new UnsubscribeViewModel(email, "Дневник успешно остановлен", repository);
            return View(model);
        }
    }
}
