using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    //класс для view главной страницы
    public class GetDiaryViewModel
    {
        public GetDiaryViewModel()
        {
            Cities = new[] { "Казань", "Екатеринбург", "Москва" };
            WeatherСonditions = new[] { "температура", "явления", "облачность", "ветер", "давление", "влажность" };
        }

        public IEnumerable<string> Cities { get; set; }
        public IEnumerable<string> WeatherСonditions { get; set; }
    }
}
