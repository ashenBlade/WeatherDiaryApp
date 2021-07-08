using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class City
    {
        public int Id { get; set; }
        public string NameRu { get; set; }

        public string NameEn { get; set; }

        // Time zone offset according to UTC
        public int UtcOffset { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int PostalCode { get; set; }
        public List<UserCity> UserCities { get; set; }
        public List<WeatherRecord> WeatherRecords { get; set; }
    }
}