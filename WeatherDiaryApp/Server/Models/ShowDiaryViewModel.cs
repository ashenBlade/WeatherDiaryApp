using System;
using System.Collections.Generic;
using Database;
using Server.Infrastructure;

namespace Server.Models
{
    public class ShowDiaryViewModel
    {
        public ShowDiaryViewModel () { }
        public ShowDiaryViewModel(string userEmail, IWeatherDiaryRepository repository, SelectDiaryOptions options)
        {
            Records = repository.GetRecords(userEmail, options.CityName, DateTime.Now);
        }

        public IEnumerable<Common.WeatherRecord> Records { get; set; }
        public SelectDiaryOptions Options { get; set; }
    }
}
