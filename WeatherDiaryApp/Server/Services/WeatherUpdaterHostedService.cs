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
using City = Common.City;
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

        public WeatherUpdaterHostedService(Database.IWeatherDiaryRepository repository, IWeatherApiRequester apiRequester, ILogger<WeatherUpdaterHostedService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _apiRequester = apiRequester ?? throw new ArgumentNullException(nameof(apiRequester));
            _cities = _repository.GetAllCities();
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
            var groups = _repository.GetAllCities()
                                    .GroupBy(city => city.TimeZone,
                                             city => city)
                                    .Select(group => new { TimeZone = group.Key, Cities = group.ToList() })
                                    .ToList();
            return Task.Run(() =>
            {
                WaitUntilNextDayAt(_dayTimeMeasurement);
                _dayTimer = new Timer(obj =>
                                      {
                                          if (cancellationToken.IsCancellationRequested)
                                          {
                                              return;
                                          }
                                          foreach (var city in _cities)
                                          {
                                              var indicator = _apiRequester.GetRecord(city.Name);
                                              var record = new Common.WeatherRecord()
                                                           {
                                                               City = city,
                                                               Date = DateTime.Today,
                                                               TimeOfDay = GetTimesOfDay(DateTime.Now),
                                                               WeatherIndicator = indicator
                                                           };
                                              _repository.SaveRecord(record);
                                          }
                                      },
                                      groups,
                                      TimeSpan.Zero,
                                      TimeSpan.FromDays(1));
                _eveningTimer = new Timer(obj =>
                                          {
                                              if (cancellationToken.IsCancellationRequested)
                                              {
                                                  return;
                                              }
                                              foreach (var city in _cities)
                                              {
                                                  var indicator = _apiRequester.GetRecord(city.Name);
                                                  var record = new Common.WeatherRecord()
                                                               {
                                                                   City = city,
                                                                   Date = DateTime.Today,
                                                                   TimeOfDay = GetTimesOfDay(DateTime.Now),
                                                                   WeatherIndicator = indicator
                                                               };
                                                  _repository.SaveRecord(record);
                                              }
                                          },
                                          groups,
                                          TimeSpan.FromHours(6),
                                          TimeSpan.FromDays(1));
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}