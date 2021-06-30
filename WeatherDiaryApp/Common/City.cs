using System;

namespace Common
{
    public class City
    {
        public City() { }
        public City(string name, TimeSpan timeZone)
        {
            Name = name;
            TimeZone = timeZone;
        }

        public string Name { get; set; }
        public TimeSpan TimeZone { get; set; }
    }
}