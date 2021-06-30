using System;
using CsvHelper.Configuration.Attributes;

namespace Database
{
    public class WeatherRecord
    {
        public City City { get; set; }
        public int CityId { get; set; }

        [Format("dd.MM.yyyy")]
        public DateTime Date { get; set; }
        public int Id { get; set; }

        public TimesOfDay TimeOfDay { get; set; }

        public WeatherIndicator WeatherIndicator { get; set; }
    }
}