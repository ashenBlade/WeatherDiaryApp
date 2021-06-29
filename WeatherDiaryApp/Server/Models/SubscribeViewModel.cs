using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    //класс для view главной страницы
    public class SubscribeViewModel
    {
        public SubscribeViewModel(string successMessage = null)
        {
            Citys = new[] { "Москва", "Санкт-Петербург", "Казань", "Екатеринбург" };
            SuccessMessage = successMessage;
        }

        public IEnumerable<string> Citys { get; set; }
        public string SuccessMessage { get; set; }
    }
}
