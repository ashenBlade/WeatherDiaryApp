using Microsoft.EntityFrameworkCore;
using WeatherDiary.Data;
using Server.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class ConnectionUsersWithCities
    {
        public City City { get; set; }

        public DateTime? DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public int Id { get; set; }
        public User User { get; set; }
    }
}