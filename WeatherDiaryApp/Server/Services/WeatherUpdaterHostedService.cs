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
using Phenomena = Database.Phenomena;
using Precipitation = Database.Precipitation;
using TimesOfDay = Common.TimesOfDay;
using WindDirection = Database.WindDirection;

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


        private Task WaitUntil12()
        {
            var nextDay = DateTime.Today.AddDays(1) + _dayTimeMeasurement;
            var waitTime = nextDay - DateTime.Now;
            return Task.Delay(waitTime);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // await WaitUntil12();
            _dayTimer = new Timer(obj =>
            {
                SaveRecords(_factory);
                Thread.Sleep(TimeSpan.FromHours(6));
                SaveRecords(_factory);
            }, cancellationToken, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private static void SaveRecords(IServiceScopeFactory factory)
        {
            using var scope = factory.CreateScope();
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
                              TimeOfDay = GetTimesOfDay(DateTime.Now.TimeOfDay),
                              WeatherIndicator = ConvertToDatabaseIndicator(resp)
                          };
                context.WeatherRecords.Add(rec);
            }

            context.SaveChanges();
        }

        private static Database.WeatherIndicator ConvertToDatabaseIndicator(Common.WeatherIndicator indicator)
        {
            var phenomena = ConvertPhenomena(indicator.Phenomena);
            var precipitaion = ConvertPrecipitation(indicator.Precipitation);
            var windDir = ConvertWindDirection(indicator.WindDirection);
            var cloudy = ConvertCloudy(indicator.Cloudy);
            return new Database.WeatherIndicator()
                   {
                       Cloudy = cloudy,
                       Temperature = indicator.Temperature,
                       Phenomena = phenomena,
                       Precipitation = precipitaion,
                       Pressure = indicator.Pressure,
                       WindDirection = windDir,
                       WindSpeed = indicator.WindSpeed,
                   };
        }

        private static Database.Cloudy ConvertCloudy(Common.Cloudy indicatorCloudy)
        {
            return indicatorCloudy switch
                   {
                       Common.Cloudy.Cloudless    => Cloudy.Cloudless,
                       Common.Cloudy.AverageCloud => Cloudy.AverageCloud,
                       Common.Cloudy.PartlyCloud  => Cloudy.PartlyCloud,
                       Common.Cloudy.SolidCloud   => Cloudy.SolidCloud,
                   };
        }

        private static Database.TimesOfDay GetTimesOfDay(TimeSpan time)
        {
            return time > TimeSpan.FromHours(16)
                       ? Database.TimesOfDay.Evening
                       : Database.TimesOfDay.Day;
        }

        private static Database.WindDirection ConvertWindDirection(Common.WindDirection direction)
        {
            return direction switch
                   {
                       Common.WindDirection.E  => WindDirection.E,
                       Common.WindDirection.N  => WindDirection.N,
                       Common.WindDirection.S  => WindDirection.S,
                       Common.WindDirection.W  => WindDirection.W,
                       Common.WindDirection.NE => WindDirection.NE,
                       Common.WindDirection.NW => WindDirection.NW,
                       Common.WindDirection.SE => WindDirection.SE,
                       Common.WindDirection.SW => WindDirection.SW,
                       _                       => WindDirection.N
                   };
        }

        private static Database.Precipitation ConvertPrecipitation(Common.Precipitation precipitation)
        {
            return precipitation switch
                   {
                       Common.Precipitation.Drizzle    => Precipitation.Drizzle,
                       Common.Precipitation.Hail       => Precipitation.Hail,
                       Common.Precipitation.None       => Precipitation.None,
                       Common.Precipitation.Rain       => Precipitation.Rain,
                       Common.Precipitation.Snow       => Precipitation.Snow,
                       Common.Precipitation.SnowGroats => Precipitation.SnowGroats,
                       _                               => Precipitation.None
                   };
        }

        private static Database.Phenomena ConvertPhenomena(Common.Phenomena phenomena)
        {
            return phenomena switch
                   {
                       Common.Phenomena.Dew          => Phenomena.Dew,
                       Common.Phenomena.Fog          => Phenomena.Fog,
                       Common.Phenomena.Ice          => Phenomena.Ice,
                       Common.Phenomena.None         => Phenomena.None,
                       Common.Phenomena.Thunderstorm => Phenomena.Thunderstorm,
                       Common.Phenomena.Snowstorm    => Phenomena.Snowstorm,
                       Common.Phenomena.Hoarfrost    => Phenomena.Hoarfrost,
                       _                             => Phenomena.None
                   };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}