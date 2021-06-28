using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class User
    {
        public ICollection<City> Cities { get; set; } = new List<City>();
        public List<Connection> Connections { get; set; } = new List<Connection>();

        [Required]
        public string Email { get; set; }

        public int Id { get; set; }

        [Required]
        public string Password { get; set; }
    }
}