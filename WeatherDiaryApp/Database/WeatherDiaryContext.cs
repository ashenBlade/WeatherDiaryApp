using System;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class WeatherDiaryContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WeatherRecord> WeatherRecords { get; set; }

        public WeatherDiaryContext (DbContextOptions<WeatherDiaryContext> options)
                    : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Cities)
                .WithMany(c => c.Users)
                .UsingEntity<Connection>(
                    j => j
                        .HasOne(pt => pt.City)
                        .WithMany(t => t.Connections)
                        .HasForeignKey(pt => pt.CityId),
                    j => j
                        .HasOne(pt => pt.User)
                        .WithMany(t => t.Connections)
                        .HasForeignKey(pt => pt.UserId),
                    j =>
                    {
                        j.Property(pt => pt.DateStart).HasDefaultValue(DateTime.Now);
                        j.HasKey(t => new { t.CityId, t.UserId });
                        j.ToTable("Connections");
                    }
                    );
        }
    }
}