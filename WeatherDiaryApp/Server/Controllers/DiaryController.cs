using Common;
using Database;
using Microsoft.AspNetCore.Mvc;
using Server.Infrastructure;
using Server.Models;

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
            var model = new SubscribeViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Subscribe(string currentCity)
        {

            var model = new SubscribeViewModel(successMessage: "Дневник успешно добавлен");

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
            var model = new UnsubscribeViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Unsubscribe(string currentCity)
        {
            var model = new UnsubscribeViewModel(successMessage: "Дневник успешно остановлен");

            return View(model);
        }
    }
}
