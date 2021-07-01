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
                initializer.SeedCity(context);
            }

            if (!context.WeatherRecords.Any())
            {
                var initializer = new Initializer();
                initializer.SeedWeather(context);
            }
        }

        public Common.User AddUser (string email, string password)
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

        public List<Common.City> GetAllCities ()
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
                .Select(c => c.Name)
                .ToList();
        }

        public Common.City GetCity (string name)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var city = context.Cities.FirstOrDefault(c => c.Name == name);
            if (city is null)
            {
                return null;
            }
            return ConvertToCommon(city);
        }

        public List<Common.WeatherRecord> GetRecords (string userEmail, string cityName, DateTime date)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
            var city = context.Cities.FirstOrDefault(c => c.Name == cityName);
            if (user is null || city is null)
            {
                return null;
            }
            var userCity = context.UserCities
                .Include(uc => uc.City)
                    .ThenInclude(c => c.WeatherRecords)
                        .ThenInclude(wr => wr.WeatherIndicator)
                .FirstOrDefault(uc =>
                    uc.UserId == user.Id &&
                    uc.CityId == city.Id &&
                    uc.DateStart <= date &&
                    (!uc.DateEnd.HasValue || uc.DateEnd.Value >= date));
            if (userCity is null)
            {
                return new List<Common.WeatherRecord>();
            }
            return userCity.City.WeatherRecords
                .Where(wr =>
                    wr.Date >= userCity.DateStart &&
                    (!userCity.DateEnd.HasValue || wr.Date <= userCity.DateEnd))
                .Select(wr => ConvertToCommon(wr))
                .ToList();
        }

        public List<Common.WeatherRecord> GetRecords (string userEmail, string cityName)
        {
            using var context = new WeatherDiaryContext(ContextOptions);
            var user = context.Users.FirstOrDefault(u => u.Email == userEmail);
            var city = context.Cities.FirstOrDefault(c => c.Name == cityName);
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
                    .Reference(wr => wr.WeatherIndicator)
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
                .Select(uc => uc.City.Name)
                .ToList();
        }

        public Common.User GetUser (string email, string password)
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

        public void SaveRecord (Common.WeatherRecord record)
        {
            var databaseRecord = ConvertToDatabase(record);
            using var context = new WeatherDiaryContext(ContextOptions);
            var city = context.Cities.FirstOrDefault(c => c.Name == databaseRecord.City.Name);
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
            var city = context.Cities.FirstOrDefault(c => c.Name == cityName);
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
            var city = context.Cities.FirstOrDefault(c => c.Name == cityName);
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

        private Common.City ConvertToCommon (City city)
        {
            return new Common.City { Name = city.Name, TimeZone = TimeSpan.FromHours(city.TimeZone) };
        }

        private Common.WeatherIndicator ConvertToCommon (WeatherIndicator weatherIndicator)
        {
            return new Common.WeatherIndicator
            {
                Cloudy = (Common.Cloudy)weatherIndicator.Cloudy,
                Phenomena = (Common.Phenomena)weatherIndicator.Phenomena,
                Precipitation = (Common.Precipitation)weatherIndicator.Precipitation,
                Pressure = weatherIndicator.Pressure,
                Temperature = weatherIndicator.Temperature,
                WindDirection = (Common.WindDirection)weatherIndicator.WindDirection,
                WindSpeed = weatherIndicator.WindSpeed
            };
        }

        private Common.User ConvertToCommon (User user)
        {
            return new Common.User { Email = user.Email, Password = user.Password };
        }

        private Common.WeatherRecord ConvertToCommon (WeatherRecord weatherRecord)
        {
            return new Common.WeatherRecord
            {
                City = ConvertToCommon(weatherRecord.City),
                Date = weatherRecord.Date,
                TimeOfDay = (Common.TimesOfDay)weatherRecord.TimeOfDay,
                WeatherIndicator = ConvertToCommon(weatherRecord.WeatherIndicator)
            };
        }

        private City ConvertToDatabase (Common.City city)
        {
            return new City { Name = city.Name, TimeZone = city.TimeZone.Hours };
        }

        private WeatherRecord ConvertToDatabase (Common.WeatherRecord weatherRecord)
        {
            return new WeatherRecord
            {
                City = ConvertToDatabase(weatherRecord.City),
                Date = weatherRecord.Date,
                TimeOfDay = (TimesOfDay)weatherRecord.TimeOfDay,
                WeatherIndicator = ConvertToDatabase(weatherRecord.WeatherIndicator)
            };
        }

        private WeatherIndicator ConvertToDatabase (Common.WeatherIndicator weatherIndicator)
        {
            return new WeatherIndicator
            {
                Cloudy = (Cloudy)weatherIndicator.Cloudy,
                Phenomena = (Phenomena)weatherIndicator.Phenomena,
                Precipitation = (Precipitation)weatherIndicator.Precipitation,
                Pressure = weatherIndicator.Pressure,
                Temperature = weatherIndicator.Temperature,
                WindDirection = (WindDirection)weatherIndicator.WindDirection,
                WindSpeed = weatherIndicator.WindSpeed
            };
        }
    }
}