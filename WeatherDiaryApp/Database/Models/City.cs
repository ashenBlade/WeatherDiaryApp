using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class City
    {
        public List<Connection> Connections { get; set; } = new List<Connection>();
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Region Region { get; set; }

        public int RegionId { get; set; }

        [Required]
        public DateTime Time { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public List<WeatherRecord> WeatherRecords { get; set; } = new List<WeatherRecord>();
    }
}