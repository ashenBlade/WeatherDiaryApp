using System.Collections.Generic;
using System.Linq;
using Database;
using Server.Infrastructure;

namespace Server.Models
{
    //класс для view главной страницы
    public class SelectDiaryViewModel
    {

        public SelectDiaryViewModel(string userEmail, IWeatherDiaryRepository repository)
        {
            Cities = repository.GetSubscribedCityNamesForUser(userEmail).OrderBy(c => c);
            WeatherСonditions = new[]
                                {
                                    new WeatherCondition() { NameEn = "Temperature", NameRu = "Температура" },
                                    new WeatherCondition() { NameEn = "Precipitations", NameRu = "Осадки" },
                                    new WeatherCondition() { NameEn = "Pressure", NameRu = "Давление" },
                                    new WeatherCondition() { NameEn = "Wind", NameRu = "Ветер" },
                                    new WeatherCondition() { NameEn = "Cloudy", NameRu = "Облачность" },
                                    new WeatherCondition() { NameEn = "Phenomena", NameRu = "Явление" }
                                };
        }

        public IEnumerable<string> Cities { get; set; }
        public IEnumerable<WeatherCondition> WeatherСonditions { get; set; }
    }
}