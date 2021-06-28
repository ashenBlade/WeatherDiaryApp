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
        public IActionResult Subscribe(string currentCity)
        {
            var model = new CreateVM();
            return View(model);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var model = new HomeVM();
            return View(model);
        }

        [HttpPost]
        public IActionResult Get(string currentCity, TimesOfDay timeOfDay)
        {
            var model = new HomeVM();
            return View(model);
        }

        [HttpGet]
        public IActionResult Unsubscribe()
        {
            var model = new StopVM();
            return View(model);
        }

        [HttpPost]
        public IActionResult Unsubscribe(string currentCity)
        {
            var model = new StopVM();
            return View(model);
        }
    }
}
