using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    //класс для view главной страницы
    public class UnsubscribeViewModel
    {
        public UnsubscribeViewModel(string successMessage = null)
        {
            Citys = new[] { "Казань", "Екатеринбург", "Москва" };
            SuccessMessage = successMessage;
        }

        public IEnumerable<string> Citys { get; set; }
        public string SuccessMessage { get; set; }
    }
}
