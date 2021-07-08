using System;

namespace WeatherDiary.Domain
{
    public class WeatherRecord
    {
        public WeatherRecord() { }
        public WeatherRecord(City city, DateTime date, TimesOfDay timeOfDay, WeatherStamp weatherStamp)
        {
            City = city;
            Date = date;
            TimeOfDay = timeOfDay;
            WeatherStamp = weatherStamp;
        }

        public City City { get; set; }
        public DateTime Date { get; set; }
        public TimesOfDay TimeOfDay { get; set; }
        public WeatherStamp WeatherStamp { get; set; }
    }
}