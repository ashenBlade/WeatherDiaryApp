using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WeatherDiary.Api;
using WeatherDiary.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherDiary.Data;
using City = WeatherDiary.Domain.City;
using Cloudy = WeatherDiary.Data.Models.Cloudy;
using Phenomena = WeatherDiary.Data.Models.Phenomena;
using Precipitation = WeatherDiary.Data.Models.Precipitation;
using TimesOfDay = WeatherDiary.Domain.TimesOfDay;
using WeatherRecord = WeatherDiary.Data.Models.WeatherRecord;
using WeatherStamp = WeatherDiary.Data.Models.WeatherStamp;
using WindDirection = WeatherDiary.Data.Models.WindDirection;

namespace WeatherDiary.Server.Services
{
    public class WeatherUpdaterHostedService : IHostedService
    {
        private IWeatherDiaryRepository _repository;
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

        private TimeSpan TimeUntil12 =>
            DateTime.Today.AddDays(1) + _dayTimeMeasurement - DateTime.Now;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _dayTimer = new Timer(obj =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                SaveRecords(_factory);
                Thread.Sleep(TimeSpan.FromHours(6));
                SaveRecords(_factory);
            }, cancellationToken, TimeUntil12, TimeSpan.FromDays(1));
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
                var resp = api.GetRecord(city.NameRu);
                var rec = new WeatherRecord()
                          {
                              City = city,
                              CityId = city.Id,
                              Date = DateTime.Today,
                              TimeOfDay = GetTimesOfDay(DateTime.Now.TimeOfDay),
                              WeatherStamp = ConvertToDatabaseIndicator(resp)
                          };
                context.WeatherRecords.Add(rec);
            }

            context.SaveChanges();
        }

        private static WeatherStamp ConvertToDatabaseIndicator(Domain.WeatherStamp stamp)
        {
            var phenomena = ConvertPhenomena(stamp.Phenomena);
            var precipitaion = ConvertPrecipitation(stamp.Precipitation);
            var windDir = ConvertWindDirection(stamp.WindDirection);
            var cloudy = ConvertCloudy(stamp.Cloudy);
            return new WeatherStamp()
                   {
                       Cloudy = cloudy,
                       Temperature = stamp.Temperature,
                       Phenomena = phenomena,
                       Precipitation = precipitaion,
                       Pressure = stamp.Pressure,
                       WindDirection = windDir,
                       WindSpeed = stamp.WindSpeed,
                   };
        }

        private static Cloudy ConvertCloudy(Domain.Cloudy indicatorCloudy)
        {
            return indicatorCloudy switch
                   {
                       Domain.Cloudy.Cloudless    => Cloudy.Cloudless,
                       Domain.Cloudy.AverageCloud => Cloudy.AverageCloud,
                       Domain.Cloudy.PartlyCloud  => Cloudy.PartlyCloud,
                       Domain.Cloudy.SolidCloud   => Cloudy.SolidCloud,
                   };
        }

        private static Data.Models.TimesOfDay GetTimesOfDay(TimeSpan time)
        {
            return time > TimeSpan.FromHours(16)
                       ? Data.Models.TimesOfDay.Evening
                       : Data.Models.TimesOfDay.Day;
        }

        private static WindDirection ConvertWindDirection(Domain.WindDirection direction)
        {
            return direction switch
                   {
                       Domain.WindDirection.E  => WindDirection.E,
                       Domain.WindDirection.N  => WindDirection.N,
                       Domain.WindDirection.S  => WindDirection.S,
                       Domain.WindDirection.W  => WindDirection.W,
                       Domain.WindDirection.NE => WindDirection.NE,
                       Domain.WindDirection.NW => WindDirection.NW,
                       Domain.WindDirection.SE => WindDirection.SE,
                       Domain.WindDirection.SW => WindDirection.SW,
                       _                       => WindDirection.N
                   };
        }

        private static Precipitation ConvertPrecipitation(Domain.Precipitation precipitation)
        {
            return precipitation switch
                   {
                       Domain.Precipitation.Drizzle    => Precipitation.Drizzle,
                       Domain.Precipitation.Hail       => Precipitation.Hail,
                       Domain.Precipitation.None       => Precipitation.None,
                       Domain.Precipitation.Rain       => Precipitation.Rain,
                       Domain.Precipitation.Snow       => Precipitation.Snow,
                       Domain.Precipitation.SnowGroats => Precipitation.SnowGroats,
                       _                               => Precipitation.None
                   };
        }

        private static Phenomena ConvertPhenomena(Domain.Phenomena phenomena)
        {
            return phenomena switch
                   {
                       Domain.Phenomena.Dew          => Phenomena.Dew,
                       Domain.Phenomena.Fog          => Phenomena.Fog,
                       Domain.Phenomena.Ice          => Phenomena.Ice,
                       Domain.Phenomena.None         => Phenomena.None,
                       Domain.Phenomena.Thunderstorm => Phenomena.Thunderstorm,
                       Domain.Phenomena.Snowstorm    => Phenomena.Snowstorm,
                       Domain.Phenomena.Hoarfrost    => Phenomena.Hoarfrost,
                       _                             => Phenomena.None
                   };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}