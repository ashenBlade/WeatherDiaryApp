using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherDiary.Data;
using Server.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace WeatherDiary.Data
{
    public static class SeedData
    {
        public static void Initialize (IServiceProvider serviceProvider)
        {
            using (var context = new WeatherDiaryContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<WeatherDiaryContext>>()))
            {
                // Look for any movies.
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            Email = "ololo@mail.ru",
                            Password = "123"
                        },
                        new User
                        {
                            Email = "alala@mail.ru",
                            Password = "123"
                        },
                        new User
                        {
                            Email = "bububu@mail.ru",
                            Password = "123"
                        });
                    context.SaveChanges();
                }

                if (!context.Cities.Any())
                {
                    context.Cities.AddRange(
                        new City
                        {
                            Name = "Kazan"
                        },
                        new City
                        {
                            Name = "Moscow"
                        },
                        new City
                        {
                            Name = "Saint-Petersburg"
                        });
                    context.SaveChanges();
                }

                if (!context.Regions.Any())
                {
                    context.Regions.AddRange(
                        new Region
                        {
                            Name = "Republic Tatarstan",
                            Cities = new List<City> { context.Cities.FirstOrDefault(x => x.Name == "Kazan") }
                        },
                        new Region
                        {
                            Name = "Moscow Region",
                            Cities = new List<City> { context.Cities.FirstOrDefault(x => x.Name == "Moscow") }
                        },
                        new Region
                        {
                            Name = "Leningrad Region",
                            Cities = new List<City> { context.Cities.FirstOrDefault(x => x.Name == "Saint-Petersburg") }
                        });
                    context.SaveChanges();
                }

                if (!context.Connections.Any())
                {
                    context.Connections.AddRange(
                        new ConnectionUsersWithCities
                        {
                            User = context.Users.Find(1),
                            City = context.Cities.Find(1)
                        },
                        new ConnectionUsersWithCities
                        {
                            User = context.Users.Find(2),
                            City = context.Cities.Find(2)
                        },
                        new ConnectionUsersWithCities
                        {
                            User = context.Users.Find(3),
                            City = context.Cities.Find(3)
                        });
                    context.SaveChanges();
                }

                if (!context.WeatherRecords.Any())
                {
                    context.WeatherRecords.AddRange(
                        new WeatherRecord
                        {
                            City = context.Cities.Find(1),
                            Date = new DateTime(2021, 6, 26),
                            WeatherIndicator = new WeatherIndicator(),
                            TimeOfDay = TimesOfDay.Day
                        },
                        new WeatherRecord
                        {
                            City = context.Cities.Find(1),
                            Date = new DateTime(2021, 6, 26),
                            WeatherIndicator = new WeatherIndicator(),
                            TimeOfDay = TimesOfDay.Evening
                        },
                        new WeatherRecord
                        {
                            City = context.Cities.Find(2),
                            Date = new DateTime(2021, 6, 26),
                            WeatherIndicator = new WeatherIndicator(),
                            TimeOfDay = TimesOfDay.Day
                        },
                        new WeatherRecord
                        {
                            City = context.Cities.Find(2),
                            Date = new DateTime(2021, 6, 27),
                            WeatherIndicator = new WeatherIndicator(),
                            TimeOfDay = TimesOfDay.Evening
                        },
                        new WeatherRecord
                        {
                            City = context.Cities.Find(3),
                            Date = new DateTime(2021, 6, 24),
                            WeatherIndicator = new WeatherIndicator(),
                            TimeOfDay = TimesOfDay.Day
                        },
                        new WeatherRecord
                        {
                            City = context.Cities.Find(3),
                            Date = new DateTime(2021, 6, 25),
                            WeatherIndicator = new WeatherIndicator(),
                            TimeOfDay = TimesOfDay.Evening
                        });
                    context.SaveChanges();
                }
            }
        }
    }
}