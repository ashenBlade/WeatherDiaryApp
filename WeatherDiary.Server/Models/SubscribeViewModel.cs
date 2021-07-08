using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherDiary.Data;

namespace WeatherDiary.Server
{
    //класс для view главной страницы
    public class SubscribeViewModel
    {
        public SubscribeViewModel() { }

        public SubscribeViewModel(string userEmail, IWeatherDiaryRepository repository, string successMessage = null, string errorMessage = null)
        {
            Cities = repository.GetAllCityNames().OrderBy(c => c).Except(repository.GetSubscribedCityNamesForUser(userEmail));
            SuccessMessage = successMessage;
            ErrorMessage = errorMessage;
        }

        public IEnumerable<string> Cities { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
