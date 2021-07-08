using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeatherDiary.Data.Models;
using WeatherDiary.Data.SeedData;

namespace WeatherDiary.Data
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
                initializer.SeedCity(context);
            }

            if (!context.WeatherRecords.Any())
            {
                var initializer = new Initializer();
                initializer.SeedWeather(context);
            }
        }

        public Domain.User AddUser (string email, string password)
        {
            if (email is null || password is null)
            {
                return null;
            }
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = new User { Email = email, Password = password };
            context.Users.Add(user);
            context.SaveChanges();
            return ConvertToCommon(user);
        }

        public bool ContainsUser (string email)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            return context.Users
                .Any(x => x.Email == email);
        }

        public List<Domain.City> GetAllCities ()
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            return context.Cities
                .Select(ConvertToCommon)
                .ToList();
        }

        public List<string> GetAllCityNames ()
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            return context.Cities
                .Select(c => c.NameRu)
                .ToList();
        }

        public Domain.City GetCity (string name)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var city = context.Cities.FirstOrDefault(c => c.NameRu == name);
            if (city is null)
            {
                return null;
            }
            return ConvertToCommon(city);
        }

        public List<Domain.WeatherRecord> GetRecords (string userEmail, string cityName, DateTime date)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
            var city = context.Cities.FirstOrDefault(c => c.NameRu == cityName);
            if (user is null || city is null)
            {
                return null;
            }
            var userCity = context.UserCities
                .Include(uc => uc.City)
                    .ThenInclude(c => c.WeatherRecords)
                        .ThenInclude(wr => wr.WeatherStamp)
                .FirstOrDefault(uc =>
                    uc.UserId == user.Id &&
                    uc.CityId == city.Id &&
                    uc.DateStart <= date &&
                    (!uc.DateEnd.HasValue || uc.DateEnd.Value >= date));
            if (userCity is null)
            {
                return new List<Domain.WeatherRecord>();
            }
            return userCity.City.WeatherRecords
                .Where(wr =>
                    wr.Date >= userCity.DateStart &&
                    (!userCity.DateEnd.HasValue || wr.Date <= userCity.DateEnd))
                .Select(wr => ConvertToCommon(wr))
                .ToList();
        }

        public List<Domain.WeatherRecord> GetRecords (string userEmail, string cityName)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
            var city = context.Cities.FirstOrDefault(c => c.NameRu == cityName);
            if (user is null || city is null)
            {
                return null;
            }

            var userCity = context.UserCities
                .FirstOrDefault(uc => uc.UserId == user.Id && uc.CityId == city.Id && !uc.DateEnd.HasValue);
            if (userCity is null)
            {
                return null;
            }
            context.Entry(userCity)
                .Reference(uc => uc.City)
                .Load();
            context.Entry(userCity.City)
                .Collection(c => c.WeatherRecords)
                .Load();
            foreach (var wr in userCity.City.WeatherRecords)
            {
                context.Entry(wr)
                    .Reference(wr => wr.WeatherStamp)
                    .Load();
                context.Entry(wr)
                    .Reference(wr => wr.City)
                    .Load();
            }
            return userCity.City.WeatherRecords
                .Where(wr => wr.Date >= userCity.DateStart)
                .Select(ConvertToCommon)
                .ToList();
        }

        public List<string> GetSubscribedCityNamesForUser (string userEmail)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users
                .FirstOrDefault(u => u.Email == userEmail);
            if (user is null)
            {
                return new List<string>();
            }
            context.Entry(user)
                .Collection(u => u.UserCities)
                .Load();
            foreach (var uc in user.UserCities)
            {
                context.Entry(uc)
                    .Reference(uc => uc.City)
                    .Load();
            }
            return user.UserCities
                .Where(uc => !uc.DateEnd.HasValue)
                .Select(uc => uc.City.NameRu)
                .ToList();
        }

        public Domain.User GetUser (string email, string password)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users
                .FirstOrDefault(user =>
                    user.Email == email &&
                    user.Password == password);
            if (user is null)
            {
                return null;
            }
            return ConvertToCommon(user);
        }

        public void SaveRecord (Domain.WeatherRecord record)
        {
            var databaseRecord = ConvertToDatabase(record);
            using var context = new WeatherDiaryContext(ContextOptions);
            var city = context.Cities.FirstOrDefault(c => c.NameRu == databaseRecord.City.NameRu);
            if (city is null)
            {
                return;
            }
            databaseRecord.City = city;
            context.WeatherRecords.Add(databaseRecord);
            context.SaveChanges();
        }

        public void StartDiary (string userEmail, string cityName)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
            var city = context.Cities.FirstOrDefault(c => c.NameRu == cityName);
            if (user is null || city is null)
            {
                return;
            }
            context.UserCities.Add(new UserCity
            {
                User = user,
                City = city,
                DateStart = DateTime.Now
            });
            context.SaveChanges();
        }

        public void StopDiary (string userEmail, string cityName)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
            var city = context.Cities.FirstOrDefault(c => c.NameRu == cityName);
            if (user is null || city is null)
            {
                return;
            }
            var userCity = context.UserCities.FirstOrDefault(uc =>
                uc.UserId == user.Id &&
                uc.CityId == city.Id &&
                !uc.DateEnd.HasValue);
            if (userCity is null)
            {
                return;
            }
            userCity.DateEnd = DateTime.Now;
            context.SaveChanges();
        }

        private Domain.City ConvertToCommon (City city)
        {
            return new Domain.City(city.NameRu, TimeSpan.FromHours(city.UtcOffset));
        }

        private Domain.WeatherStamp ConvertToCommon (WeatherStamp weatherStamp)
        {
            return new Domain.WeatherStamp
           (
                (Domain.Cloudy)weatherStamp.Cloudy,
                (Domain.Phenomena)weatherStamp.Phenomena,
                (Domain.Precipitation)weatherStamp.Precipitation,
                weatherStamp.Pressure,
                weatherStamp.Temperature,
                (Domain.WindDirection)weatherStamp.WindDirection,
                weatherStamp.WindSpeed
            );
        }

        private Domain.User ConvertToCommon (User user)
        {
            return new Domain.User (user.Email, user.Password);
        }

        private Domain.WeatherRecord ConvertToCommon (WeatherRecord weatherRecord)
        {
            return new Domain.WeatherRecord
            {
                City = ConvertToCommon(weatherRecord.City),
                Date = weatherRecord.Date,
                TimeOfDay = (Domain.TimesOfDay)weatherRecord.TimeOfDay,
                WeatherStamp = ConvertToCommon(weatherRecord.WeatherStamp)
            };
        }

        private City ConvertToDatabase (Domain.City city)
        {
            return new City { NameRu = city.Name, UtcOffset = city.TimeZone.BaseUtcOffset.Hours };
        }

        private WeatherRecord ConvertToDatabase (Domain.WeatherRecord weatherRecord)
        {
            return new WeatherRecord
            {
                City = ConvertToDatabase(weatherRecord.City),
                Date = weatherRecord.Date,
                TimeOfDay = (TimesOfDay)weatherRecord.TimeOfDay,
                WeatherStamp = ConvertToDatabase(weatherRecord.WeatherStamp)
            };
        }

        private WeatherStamp ConvertToDatabase (Domain.WeatherStamp weatherStamp)
        {
            return new WeatherStamp
            {
                Cloudy = (Cloudy)weatherStamp.Cloudy,
                Phenomena = (Phenomena)weatherStamp.Phenomena,
                Precipitation = (Precipitation)weatherStamp.Precipitation,
                Pressure = weatherStamp.Pressure,
                Temperature = weatherStamp.Temperature,
                WindDirection = (WindDirection)weatherStamp.WindDirection,
                WindSpeed = weatherStamp.WindSpeed
            };
        }
    }
}