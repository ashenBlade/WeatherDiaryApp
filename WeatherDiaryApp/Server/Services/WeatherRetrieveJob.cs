using System;
using System.Threading.Tasks;
using Api;
using Common;
using Database;
using Microsoft.Extensions.Logging;
using Quartz;
using TimesOfDay = Common.TimesOfDay;

namespace Server.Services
{
    public class WeatherRetrieveJob : IJob
    {
        private readonly IWeatherApiRequester _requester;
        private readonly ILogger<WeatherRetrieveJob> _logger;
        private readonly IWeatherDiaryRepository _repository;

        public WeatherRetrieveJob(IWeatherApiRequester requester, ILogger<WeatherRetrieveJob> logger, IWeatherDiaryRepository repository)
        {
            _requester = requester ?? throw new ArgumentNullException(nameof(requester));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task Execute(IJobExecutionContext context)
        {
            var answ = _requester.GetRecord("Moscow");
            _repository.SaveRecord(new Common.WeatherRecord()
                                   {
                                       City = new Common.City() { Name = "Казань"},
                                       Date = DateTime.Now,
                                       TimeOfDay = TimesOfDay.Evening,
                                       WeatherIndicator = answ
                                   });
            _logger.LogInformation("Weather saved for Moscow");
            return Task.CompletedTask;
        }
    }
}