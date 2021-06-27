using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace WeatherDiary.Data
{
    public class WeatherDiaryContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<ConnectionUsersWithCities> Connections { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WeatherRecord> WeatherRecords { get; set; }

        public WeatherDiaryContext (DbContextOptions<WeatherDiaryContext> options)
                    : base(options)
        {
        }
    }
}