using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public class UserCity
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// Date of starting diary
        /// </summary>
        public DateTime DateStart { get; set; }
        /// <summary>
        /// Date of ending diary
        /// </summary>
        /// <remarks>Null if diary is currently tracking</remarks>
        public DateTime? DateEnd { get; set; }
    }
}