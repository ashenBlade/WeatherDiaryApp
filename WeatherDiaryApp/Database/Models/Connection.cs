using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public class Connection
    {
        public City City { get; set; }
        public int CityId { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}