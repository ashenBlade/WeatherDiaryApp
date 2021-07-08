using System;
using CsvHelper.Configuration.Attributes;

namespace Database
{
    public class WeatherRecord
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        [Format("dd.MM.yyyy")]
        public DateTime Date { get; set; }
        public TimesOfDay TimeOfDay { get; set; }
        public WeatherStamp WeatherStamp { get; set; }
    }
}