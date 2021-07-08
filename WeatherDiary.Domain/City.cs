using System;

namespace WeatherDiary.Domain
{
    public class City
    {
        public City(string name, TimeSpan utcOffset)
        {
            Name = name;
            TimeZone = TimeZoneInfo.CreateCustomTimeZone(utcOffset.ToString(), utcOffset, null, null);
        }
        public TimeZoneInfo TimeZone { get; private set; }
        public string Name { get; private set; }
    }
}