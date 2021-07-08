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
        public UnsubscribeViewModel(string userEmail, IWeatherDiaryRepository repository, string successMessage = null, string errorMessage = null)
        {
            Cities = repository.GetSubscribedCityNamesForUser(userEmail).OrderBy(c => c);
            SuccessMessage = successMessage;
            ErrorMessage = errorMessage;
        }

        public IEnumerable<string> Cities { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
