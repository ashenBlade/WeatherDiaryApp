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
            var model = new CreateVM();
            return View(model);
        }

        [HttpPost]
        public string Subscribe(string currentCity)
        {
            return "Subscribe";
        }

        [HttpGet]
        public IActionResult Get()
        {
            var model = new HomeVM();
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
            var model = new StopVM();
            return View(model);
        }

        [HttpPost]
        public string Unsubscribe(string currentCity)
        {
            return "Unsubscribe";
        }
    }
}
