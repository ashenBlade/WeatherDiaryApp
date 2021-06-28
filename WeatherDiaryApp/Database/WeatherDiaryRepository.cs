using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Database
{
    public class WeatherDiaryRepository : IWeatherDiaryRepository
    {
        public DbContextOptions<WeatherDiaryContext> contextOptions { get; set; }

        public WeatherDiaryRepository (string connectionString)
        {
            contextOptions = new DbContextOptionsBuilder<WeatherDiaryContext>()
                .UseSqlite(connectionString)
                .Options;
        }

        public User AddUser (string email, string password)
        {
            using (var context = new WeatherDiaryContext(contextOptions))
            {
                var user = new User { Email = email, Password = password };
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }
        }

        public bool ContainsUser (string email)
        {
            using (var context = new WeatherDiaryContext(contextOptions))
            {
                return context.Users
                    .Any(x => x.Email == email);
            }
        }

        public List<WeatherRecord> GetRecords (User user, City city, DateTime date)
        {
            using (var context = new WeatherDiaryContext(contextOptions))
            {
                user = context.Users.Find(user.Id);
                city = user.Cities.FirstOrDefault(c => c.Id == city.Id);
                var connection = user.Connections.FirstOrDefault(c => c.CityId == city.Id && c.DateStart.Month <= date.Month && c.DateEnd?.Month >= date.Month);
                return city.WeatherRecords.Where(wr => wr.Date.Month >= connection.DateStart.Month && wr.Date.Month <= connection.DateEnd?.Month).ToList();
            }
        }

        public User GetUser (string email, string password)
        {
            using (var context = new WeatherDiaryContext(contextOptions))
            {
                return context.Users
                    .FirstOrDefault(user => user.Email == email && user.Password == password);
            }
        }

        public void SaveRecord (WeatherRecord record)
        {
            using (var context = new WeatherDiaryContext(contextOptions))
            {
                var city = context.Cities.Find(record.City.Id);
                city.WeatherRecords.Add(record);
                context.SaveChanges();
            }
        }

        public void StartDiary (User user, City city)
        {
            using (var context = new WeatherDiaryContext(contextOptions))
            {
                user = context.Users.Find(user.Id);
                city = context.Cities.Find(city.Id);
                user.Cities.Add(city);
                city.Users.Add(user);
                context.Users.Update(user);
                context.Cities.Update(city);
                context.SaveChanges();
            }
        }

        public void StopDiary (User user, City city)
        {
            using (var context = new WeatherDiaryContext(contextOptions))
            {
                city = context.Cities.Find(city.Id);
                var connection = city.Connections.Find(c => c.UserId == user.Id);
                connection.DateEnd = DateTime.Now;
                context.Cities.Update(city);
                user = context.Users.FirstOrDefault(u => u.Id == user.Id);
                context.Users.Update(user);
                context.SaveChanges();
            }
        }
    }
}