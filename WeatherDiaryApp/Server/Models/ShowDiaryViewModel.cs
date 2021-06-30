using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using Database;
using Server.Infrastructure;

namespace Server.Models
{
    public class ShowDiaryViewModel
    {
        public ShowDiaryViewModel(string userEmail, IWeatherDiaryRepository repository, SelectDiaryOptions options)
        {
            var fullRecords = repository.GetRecords(userEmail, options.CityName, DateTime.Now);
            ResetUnnecessaryElementsOfRecords(options, fullRecords);
        }

        public IEnumerable<Database.WeatherRecord> Records { get; set; }

        private void ResetUnnecessaryElementsOfRecords(SelectDiaryOptions options, 
            List<Database.WeatherRecord> records)
        {
            foreach (var record in records)
            {
                if (!options.Cloudy)
                    record.WeatherIndicator.Cloudy = Database.Cloudy.Empty;
                if (!options.Precipitations)
                    record.WeatherIndicator.Precipitation = Database.Precipitation.Empty;
                if (!options.Pressure)
                    record.WeatherIndicator.Pressure = null;
                if (!options.Temperature)
                    record.WeatherIndicator.Temperature = null;
                if (!options.Wind)
                {
                    record.WeatherIndicator.WindDirection = Database.WindDirection.Empty;
                    record.WeatherIndicator.WindSpeed = null;
                }
            }
            Records = records;
        }
    }
}
