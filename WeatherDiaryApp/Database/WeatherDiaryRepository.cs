using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Database.SeedData;

namespace Database
{
    public class WeatherDiaryRepository : IWeatherDiaryRepository
    {
        private DbContextOptions<WeatherDiaryContext> ContextOptions { get; set; }

        public WeatherDiaryRepository (string connectionString)
        {
            ContextOptions = new DbContextOptionsBuilder<WeatherDiaryContext>()
                .UseSqlite(connectionString)
                .Options;

            using var context = new WeatherDiaryContext(ContextOptions);
            if (!context.Cities.Any())
            {
                var initializer = new Initializer();
                initializer.Seed(context);
            }
        }

        public User AddUser (string email, string password)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = new User { Email = email, Password = password };
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public bool ContainsUser (string email)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            return context.Users
                .Any(x => x.Email == email);
        }

        public List<City> GetAllCities ()
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            return context.Cities
                .Include(c => c.UserCities)
                .Include(c => c.WeatherRecords)
                    .ThenInclude(wr => wr.WeatherIndicator)
                .ToList();
        }

        public City GetCity (string name)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            return context.Cities
                .Include(c => c.UserCities)
                    .ThenInclude(uc => uc.User)
                .Include(c => c.WeatherRecords)
                    .ThenInclude(wr => wr.WeatherIndicator)
                .FirstOrDefault(c => c.Name == name);
        }

        public List<WeatherRecord> GetRecords (User user, City city, DateTime date)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var userCity = context.UserCities
                .Include(uc => uc.City)
                    .ThenInclude(c => c.WeatherRecords)
                        .ThenInclude(wr => wr.WeatherIndicator)
                .FirstOrDefault(uc =>
                    uc.UserId == user.Id &&
                    uc.CityId == city.Id &&
                    uc.DateStart <= date &&
                    (!uc.DateEnd.HasValue || uc.DateEnd.Value >= date));
            return userCity.City.WeatherRecords
                .Where(wr =>
                    wr.Date >= userCity.DateStart &&
                    (!userCity.DateEnd.HasValue || wr.Date <= userCity.DateEnd))
                .ToList();
        }

        public User GetUser (string email, string password)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            return context.Users
                .Include(u => u.UserCities)
                    .ThenInclude(uc => uc.City)
                .FirstOrDefault(user =>
                    user.Email == email &&
                    user.Password == password);
        }

        public void SaveRecord (WeatherRecord record)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var city = context.Cities.Find(record.City.Id);
            record.City = city;
            context.WeatherRecords.Add(record);
            context.SaveChanges();
        }

        public void StartDiary (string email, City city)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            city = context.Cities.Find(city.Id);
            context.UserCities.Add(new UserCity
            {
                User = user,
                City = city,
                DateStart = DateTime.Now
            });
            context.SaveChanges();
        }

        public void StopDiary (string email, City city)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            var userCity = context.UserCities.FirstOrDefault(uc =>
                uc.UserId == user.Id &&
                uc.CityId == city.Id &&
                !uc.DateEnd.HasValue);
            userCity.DateEnd = DateTime.Now;
            context.SaveChanges();
        }
    }
}