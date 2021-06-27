using System;

namespace Server.Models
{
    class WeatherIndicator
    {
        public int Temperature { get; set; }
        public int Pressure { get; set; }
        public int WindSpeed { get; set; }
        public WindDirection WindDirection { get; set; }
        public DateTime Date { get; set; }
        public Cloudy Cloudy { get; set; }
        public Precipitation Precipitation { get; set; }
    }
}
