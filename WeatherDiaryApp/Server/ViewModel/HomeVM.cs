using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    //класс для view главной страницы
    public class HomeVM
    {
        public HomeVM()
        {
            Citys = new[] { "Казань", "Екатеринбург", "Москва" };
            WeatherСonditions = new[] { "температура", "явления", "облачность", "ветер", "давление", "влажность" };
        }

        public IEnumerable<string> Citys { get; set; }
        public IEnumerable<string> WeatherСonditions { get; set; }
    }
}
