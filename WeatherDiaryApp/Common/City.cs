using System;

namespace Common
{
    public class City
    {
        public City() { }
        public City(string name, DateTime time)
        {
            Name = name;
            Time = time;
        }

        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}