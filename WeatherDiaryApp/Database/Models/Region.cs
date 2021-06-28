using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class Region
    {
        public List<City> Cities { get; set; }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}