using System;
using System.Collections.Generic;
using Database;
using Server.Infrastructure;

namespace Server.Models
{
    public class ShowDiaryViewModel
    {
        public ShowDiaryViewModel(string userEmail, IWeatherDiaryRepository repository, SelectDiaryOptions options)
        {
            var fullRecords = repository.GetRecords(userEmail, options.CityName, DateTime.Now);
        }

        public IEnumerable<Database.WeatherRecord> Records { get; set; }
        public SelectDiaryOptions Options { get; set; }
    }
}
