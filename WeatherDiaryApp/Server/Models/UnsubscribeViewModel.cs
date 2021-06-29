using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    //класс для view главной страницы
    public class UnsubscribeViewModel
    {
        public UnsubscribeViewModel(string userEmail, string successMessage, IWeatherDiaryRepository repository)
        {
            Cities = repository.GetSubscribedCitiesForUser(userEmail).OrderBy(c => c);
            SuccessMessage = successMessage;
        }

        public IEnumerable<string> Cities { get; set; }
        public string SuccessMessage { get; set; }
    }
}
