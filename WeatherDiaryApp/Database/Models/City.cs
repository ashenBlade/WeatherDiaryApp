using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Time { get; set; }

        public List<UserCity> UserCities { get; set; }
        public List<WeatherRecord> WeatherRecords { get; set; }
    }
}