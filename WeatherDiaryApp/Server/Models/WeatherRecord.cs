using System;

namespace Server.Models
{
    public class WeatherRecord
    {
        private string city { get; set; }
        private DateTime date { get; set; }
        private TimesOfDay timeOfDay { get; set; }
        private WeatherIndicator weatherIndicator { get; set; }

        public WeatherRecord GetWeatherRecord()
        {
            return this;
        }

        public void SetWeatherRecord(string city, 
            DateTime date, 
            TimesOfDay timeOfDay, 
            WeatherIndicator weatherIndicator)
        {
            this.city = city;
            this.date = date;
            this.timeOfDay = timeOfDay;
            this.weatherIndicator = weatherIndicator;
        }
    }
}
