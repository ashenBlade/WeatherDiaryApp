using System;
using Common;
using System.Collections.Generic;
using System.Linq;
namespace Server.Models
{
    public class DiaryDetailsViewModel
    {
        public DiaryDetailsViewModel()
        {
            var indicator1 = new WeatherIndicator(Cloudy.AverageCloud, Phenomena.Fog, Precipitation.Drizzle, 
                                                  754, 17, WindDirection.E, 2);
            var record1 = new WeatherRecord(new City("Казань", DateTime.Now), DateTime.Now, TimesOfDay.Day, indicator1);
            Records.Append(record1);
            var indicator2 = new WeatherIndicator(Cloudy.PartlyCloud, Phenomena.Hoarfrost, Precipitation.Hail,
                                                  700, 40, WindDirection.NW, 30);
            var record2 = new WeatherRecord(new City("Казань", DateTime.Now), DateTime.Now, TimesOfDay.Evening, indicator2);
            Records.Append(record2);
            var indicator3 = new WeatherIndicator(Cloudy.CloudWithGaps, Phenomena.Ice, Precipitation.Snow,
                                                  777, 0, WindDirection.S, 15);
            var record3 = new WeatherRecord(new City("Казань", DateTime.Now), DateTime.Now, TimesOfDay.Evening, indicator3);
            Records.Append(record3);
        }

        public IEnumerable<WeatherRecord> Records { get; set; }
    }
}
