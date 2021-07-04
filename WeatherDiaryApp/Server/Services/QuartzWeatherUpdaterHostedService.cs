using System;
using System.Threading;
using System.Threading.Tasks;
using Api;
using Database;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Server.Services
{
    public class QuartzWeatherUpdaterHostedService : IHostedService
    {
        private const string _dayRetrieveID = "DAY_RETRIEVE";

        private readonly ILogger<QuartzWeatherUpdaterHostedService> _logger;
        private readonly IWeatherApiRequester _requester;
        private readonly IWeatherDiaryRepository _repository;

        public QuartzWeatherUpdaterHostedService(ILogger<QuartzWeatherUpdaterHostedService> logger, IWeatherDiaryRepository repository, IWeatherApiRequester requester)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _requester = requester ?? throw new ArgumentNullException(nameof(requester));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var job = JobBuilder.Create<WeatherRetrieveJob>()
                                    .WithIdentity(_dayRetrieveID)
                                    .WithDescription("Retrive weather data at day time")
                                    .Build();
            var trigger = TriggerBuilder.Create()
                                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(12, 0)
                                                                         .InTimeZone(TimeZoneInfo.Utc))
                                        .ForJob(_dayRetrieveID)
                                        .Build();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}