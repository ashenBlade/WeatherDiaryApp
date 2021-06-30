using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Api;
using Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using City = Common.City;
using Cloudy = Database.Cloudy;
using TimesOfDay = Common.TimesOfDay;

namespace Server.Services
{
    public class WeatherUpdaterHostedService : IHostedService
    {
        private Database.IWeatherDiaryRepository _repository;
        private IWeatherApiRequester _apiRequester;
        private ILogger<WeatherUpdaterHostedService> _logger;
        private List<City> _cities;
        private Timer _dayTimer;
        private Timer _eveningTimer;
        private readonly TimeSpan _dayTimeMeasurement;
        private readonly TimeSpan _eveningTimeMeasurement;
        private IServiceScopeFactory _factory;

        public WeatherUpdaterHostedService(ILogger<WeatherUpdaterHostedService> logger, IServiceScopeFactory factory)
        {
            _factory = factory;
            // _cities = _repository.GetAllCities();
            _dayTimeMeasurement = new TimeSpan(12, 0, 0);
            _eveningTimeMeasurement = new TimeSpan(18, 0, 0);
            _logger = logger;
        }

        private TimesOfDay GetTimesOfDay(DateTime time)
        {
            return time.TimeOfDay > TimeSpan.FromHours(16)
                       ? TimesOfDay.Evening
                       : TimesOfDay.Day;
        }

        private void WaitUntilNextDayAt(TimeSpan time)
        {
            var next = DateTime.Today.AddDays(1) + time;
            var waitTime = next - DateTime.Now;
            Thread.Sleep(waitTime);
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            // var groups = _repository.GetAllCities()
            //                         .GroupBy(city => city.TimeZone,
            //                                  city => city)
            //                         .Select(group => new { TimeZone = group.Key, Cities = group.ToList() })
            //                         .ToList();
            // using var scope = _factory.CreateScope();
            // var context = scope.ServiceProvider.GetRequiredService<WeatherDiaryContext>();
            var timer = new Timer(obj =>
            {
                using var scope = _factory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<WeatherDiaryContext>();
                var api = scope.ServiceProvider.GetRequiredService<IWeatherApiRequester>();
                var cities = context.Cities.ToList();
                foreach (var city in cities)
                {
                    var resp = api.GetRecord(city.Name);
                    var rec = new Database.WeatherRecord()
                              {
                                  City = city,
                                  CityId = city.Id,
                                  Date = DateTime.Today,
                                  TimeOfDay = Database.TimesOfDay.Day,
                                  WeatherIndicator = new Database.WeatherIndicator()
                                                     {
                                                         Cloudy = Cloudy.Cloudless, Pressure = 100000,
                                                     }
                              };
                    context.WeatherRecords.Add(rec);
                }

                context.SaveChanges();

            }, cancellationToken, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
            // return Task.Run(() =>
            // {
            //     WaitUntilNextDayAt(_dayTimeMeasurement);
            //     _dayTimer = new Timer(obj =>
            //                           {
            //                               if (cancellationToken.IsCancellationRequested)
            //                               {
            //                                   return;
            //                               }
            //                               foreach (var city in _cities)
            //                               {
            //                                   var indicator = _apiRequester.GetRecord(city.Name);
            //                                   var record = new Common.WeatherRecord()
            //                                                {
            //                                                    City = city,
            //                                                    Date = DateTime.Today,
            //                                                    TimeOfDay = GetTimesOfDay(DateTime.Now),
            //                                                    WeatherIndicator = indicator
            //                                                };
            //                                   _repository.SaveRecord(record);
            //                               }
            //                           },
            //                           groups,
            //                           TimeSpan.Zero,
            //                           TimeSpan.FromDays(1));
            //     _eveningTimer = new Timer(obj =>
            //                               {
            //                                   if (cancellationToken.IsCancellationRequested)
            //                                   {
            //                                       return;
            //                                   }
            //                                   foreach (var city in _cities)
            //                                   {
            //                                       var indicator = _apiRequester.GetRecord(city.Name);
            //                                       var record = new Common.WeatherRecord()
            //                                                    {
            //                                                        City = city,
            //                                                        Date = DateTime.Today,
            //                                                        TimeOfDay = GetTimesOfDay(DateTime.Now),
            //                                                        WeatherIndicator = indicator
            //                                                    };
            //                                       _repository.SaveRecord(record);
            //                                   }
            //                               },
            //                               groups,
            //                               TimeSpan.FromHours(6),
            //                               TimeSpan.FromDays(1));
            // }, cancellationToken);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}