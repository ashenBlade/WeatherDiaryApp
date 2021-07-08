using System.Collections.Generic;

namespace WeatherDiary.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<UserCity> UserCities { get; set; }
    }
}