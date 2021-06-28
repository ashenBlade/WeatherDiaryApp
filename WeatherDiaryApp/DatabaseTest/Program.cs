using System;
using Database;
using System.Linq;

namespace DatabaseTest
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            WeatherDiaryRepository repository = new WeatherDiaryRepository("Filename=Weather.db");
            using (var context = new WeatherDiaryContext(repository.contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var admin = new User { Email = "admin", Password = "123" };
                var tatarstan = new Region { Name = "tatarstan" };
                var kazan = new City { Name = "kazan" };
                context.Users.Add(admin);
                context.Cities.Add(kazan);
                context.Regions.Add(tatarstan);
                kazan.Region = tatarstan;
                admin.Cities.Add(kazan);
                var weatherRecord = new WeatherRecord { City = kazan, Date = DateTime.Now, TimeOfDay = TimesOfDay.Day, WeatherIndicator = new WeatherIndicator() };
                kazan.WeatherRecords.Add(weatherRecord);

                context.SaveChanges();
            }

            using (var context = new WeatherDiaryContext(repository.contextOptions))
            {
                var admin = context.Users.Find(1);
            }
        }
    }
}