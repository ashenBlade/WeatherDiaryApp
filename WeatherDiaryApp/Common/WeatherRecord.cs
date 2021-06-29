using System;

namespace Common
{
    public class WeatherRecord
    {
        public City City { get; set; }
        public DateTime Date { get; set; }
        public TimesOfDay TimeOfDay { get; set; }
        public WeatherIndicator WeatherIndicator { get; set; }
    }
}