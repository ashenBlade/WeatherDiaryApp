using System;

namespace Server.Models
{
    public class WeatherIndicator
    {
        public Cloudy Cloudy { get; set; }
        public int Id { get; set; }
        public Phenomena Phenomena { get; set; }
        public Precipitation Precipitation { get; set; }
        public int Pressure { get; set; }
        public int Temperature { get; set; }
        public WindDirection WindDirection { get; set; }
        public int WindSpeed { get; set; }
    }
}