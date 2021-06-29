using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    //класс для view главной страницы
    public class SubscribeViewModel
    {
        public SubscribeViewModel()
        {
            Citys = new[] { "Москва", "Санкт-Петербург", "Казань", "Екатеринбург" };
        }

        public IEnumerable<string> Citys { get; set; }
    }
}
