using System;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class WeatherRecord
    {
        public City City { get; set; }
        public int CityId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int Id { get; set; }

        [Required]
        public TimesOfDay TimeOfDay { get; set; }

        public WeatherIndicator WeatherIndicator { get; set; }
    }
}