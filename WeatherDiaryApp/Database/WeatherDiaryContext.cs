using System;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class WeatherDiaryContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<UserCity> UserCities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WeatherRecord> WeatherRecords { get; set; }

        public WeatherDiaryContext (DbContextOptions<WeatherDiaryContext> options)
                    : base(options)
        {
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserCity>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCities)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder
                .Entity<UserCity>()
                .HasOne(uc => uc.City)
                .WithMany(c => c.UserCities)
                .HasForeignKey(uc => uc.CityId);

            modelBuilder
                .Entity<WeatherRecord>()
                .HasOne(wr => wr.City)
                .WithMany(c => c.WeatherRecords)
                .HasForeignKey(wr => wr.CityId);
        }
    }
}