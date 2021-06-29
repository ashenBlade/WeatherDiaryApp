using System;

namespace Common
{
    public class WeatherRecord
    {
        public WeatherRecord() { }
        public WeatherRecord(City city, DateTime date, TimesOfDay timeOfDay, WeatherIndicator weatherIndicator)
        {
            City = city;
            Date = date;
            TimeOfDay = timeOfDay;
            WeatherIndicator = weatherIndicator;
        }

        public City City { get; set; }
        public DateTime Date { get; set; }
        public TimesOfDay TimeOfDay { get; set; }
        public WeatherIndicator WeatherIndicator { get; set; }
    }
}