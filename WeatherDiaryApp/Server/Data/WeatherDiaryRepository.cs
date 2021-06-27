using Microsoft.EntityFrameworkCore;
using WeatherDiary.Data;
using Server.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace WeatherDiary.Data
{
    public class WeatherDiaryRepository : IWeatherDiaryRepository
    {
        private WeatherDiaryContext context;

        public WeatherDiaryRepository (WeatherDiaryContext context)
        {
            this.context = context;
        }

        public bool ContainsUser (string email)
        {
            return context.Users.Any(x => x.Email == email);
        }

        public List<City> GetAllCitiesForUser (User user)
        {
            return context.Connections
                .Where(c => c.User.Id == user.Id)
                .Select(c => c.City)
                .ToList();
        }

        public List<City> GetLocationsForRecords ()
        {
            return context.Connections
                .Where(c => !c.DateEnd.HasValue)
                .Select(c => c.City)
                .ToList();
        }

        public List<WeatherRecord> GetRecords (User user, City city, DateTime date)
        {
            return context.WeatherRecords
                .Where(wr => wr.City.Id == city.Id && wr.Date.Month == date.Month)
                .ToList();
        }

        public User GetUser (string email, string password)
        {
            return context.Users
                .FirstOrDefault(user => user.Email == email && user.Password == password);
        }

        public User Register (string email, string password)
        {
            var user = new User { Email = email, Password = password };
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public void SaveRecord (WeatherRecord record)
        {
        }

        public void SaveWeatherRecord ()
        {
            throw new NotImplementedException();
        }

        public void StartDiary (User user, City city)
        {
            var newConnection = new ConnectionUsersWithCities { City = city, User = user, DateStart = DateTime.Now };
            context.Connections.Add(newConnection);
            context.SaveChanges();
        }

        public void StopDiary (User user, City city)
        {
            var connection = context.Connections.FirstOrDefault(c => c.User.Id == user.Id && c.City.Id == city.Id);
            connection.DateEnd = DateTime.Now;
            context.Connections.Update(connection);
            context.SaveChanges();
        }
    }
}