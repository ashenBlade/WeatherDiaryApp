using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    //класс для view главной страницы
    public class StopVM
    {
        public StopVM()
        {
            Citys = new[] { "Казань", "Екатеринбург", "Москва" };
        }

        public IEnumerable<string> Citys { get; set; }
    }
}
