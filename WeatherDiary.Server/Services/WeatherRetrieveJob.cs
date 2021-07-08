using System;
using System.Threading.Tasks;
using WeatherDiary.Api;
using WeatherDiary.Domain;
using Microsoft.Extensions.Logging;
using Quartz;
using WeatherDiary.Data;
using TimesOfDay = WeatherDiary.Domain.TimesOfDay;

namespace WeatherDiary.Server.Services
{
    public class WeatherRetrieveJob : IJob
    {
        private readonly IWeatherApiRequester _requester;
        private readonly ILogger<WeatherRetrieveJob> _logger;
        private readonly IWeatherDiaryRepository _repository;

        public WeatherRetrieveJob(IWeatherApiRequester requester,
                                  ILogger<WeatherRetrieveJob> logger,
                                  IWeatherDiaryRepository repository)
        {
            _requester = requester ?? throw new ArgumentNullException(nameof(requester));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Execute(IJobExecutionContext context)
        {
        }
    }
}