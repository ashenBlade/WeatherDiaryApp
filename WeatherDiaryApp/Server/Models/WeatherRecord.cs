using System;

namespace Server.Models
{
    public class WeatherRecord
    {
        public string City { get; set; }
        public DateTime Date { get; set; }
        public TimesOfDay TimeOfDay { get; set; }
        public WeatherIndicator WeatherIndicator { get; set; }
    }
}
