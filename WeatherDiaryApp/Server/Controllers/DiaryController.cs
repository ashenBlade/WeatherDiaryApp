using Common;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    public class DiaryController : Controller
    {
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
        public IActionResult Get()
        {
            var model = new GetDiaryViewModel();
            return View(model);
        }

        [HttpPost]
        public string Get(string currentCity, TimesOfDay timeOfDay)
        {
            return "Get";
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
