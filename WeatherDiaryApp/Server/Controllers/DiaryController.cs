using Common;
using Database;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
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
            var model = new SubscribeViewModel(null, repository);
            return View(model);
        }

        [HttpPost]
        public IActionResult Subscribe(SubscribeViewModel model)
        {
            string email = HttpContext.User.Identity.Name;
            model.Cities = repository.GetAllCities().OrderBy(c => c);
            model.SuccessMessage = "Дневник успешно добавлен";
            //var model = new SubscribeViewModel("Дневник успешно добавлен", repository);
            repository.StartDiary(email, model.SelectedCity);
            return View(model);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var model = new GetDiaryViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Get(string currentCity, Common.TimesOfDay timeOfDay)
        {
            var model = new GetDiaryViewModel();
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
        public IActionResult Unsubscribe(string currentCity)
        {
            string email = HttpContext.User.Identity.Name;
            repository.StopDiary(email, currentCity);
            var model = new UnsubscribeViewModel(email, "Дневник успешно остановлен", repository);
            return View(model);
        }
    }
}
