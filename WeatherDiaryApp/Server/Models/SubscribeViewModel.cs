using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    //класс для view главной страницы
    public class SubscribeViewModel
    {
        public SubscribeViewModel() { }
        public SubscribeViewModel(string userEmail, string successMessage, IWeatherDiaryRepository repository)
        {
            Cities = repository.GetAllCities().OrderBy(c => c).Except(repository.GetCitiesForUser(userEmail));
            SuccessMessage = successMessage;
        }

        public IEnumerable<string> Cities { get; set; }
        public string SuccessMessage { get; set; }
    }
}
