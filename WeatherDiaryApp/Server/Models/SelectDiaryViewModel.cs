using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Server.Infrastructure;

namespace Server
{
    //класс для view главной страницы
    public class SelectDiaryViewModel
    {
        public SelectDiaryViewModel(string userEmail, IWeatherDiaryRepository repository)
        {
            Cities = repository.GetSubscribedCitiesForUser(userEmail).OrderBy(c => c);
            WeatherСonditions = new[]
                                {
                                    new WeatherCondition() { NameEn = "Temperature", NameRu = "Температура" },
                                    new WeatherCondition() { NameEn = "Precipitations", NameRu = "Осадки" },
                                    new WeatherCondition() { NameEn = "Pressure", NameRu = "Давление" },
                                    new WeatherCondition() {NameEn = "Wind", NameRu = "Ветер" },
                                    new WeatherCondition() {NameEn = "Cloudy", NameRu = "Облачность"}
                                };
        }

        public IEnumerable<string> Cities { get; set; }
        public IEnumerable<WeatherCondition> WeatherСonditions { get; set; }
    }
}