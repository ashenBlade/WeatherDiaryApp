using System.Collections.Generic;

namespace Server.Models
{
    public class Region
    {
        public List<City> Cities { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}