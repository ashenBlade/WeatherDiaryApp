using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherDiary.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WeatherDiary.Data;

namespace WeatherDiary.Server.Services
{
    public class QuartzWeatherUpdaterHostedService : IHostedService
    {
        private const string _dayRetrieveID = "DAY_RETRIEVE";

        private readonly ILogger<QuartzWeatherUpdaterHostedService> _logger;
        private readonly IWeatherApiRequester _requester;
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IWeatherDiaryRepository _repository;
        private IScheduler _scheduler;

        public QuartzWeatherUpdaterHostedService(ILogger<QuartzWeatherUpdaterHostedService> logger,
                                                 IWeatherDiaryRepository repository,
                                                 IWeatherApiRequester requester,
                                                 IJobFactory jobFactory,
                                                 ISchedulerFactory schedulerFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _requester = requester ?? throw new ArgumentNullException(nameof(requester));
            _jobFactory = jobFactory;
            _schedulerFactory = schedulerFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var job = JobBuilder.Create<WeatherRetrieveJob>()
                                .WithIdentity(_dayRetrieveID)
                                .WithDescription("Retrive weather data at day time")
                                .Build();
            var trigger = TriggerBuilder.Create()
                                        .WithSchedule(CronScheduleBuilder.CronSchedule("0 * * ? * * *")
                                                                         .InTimeZone(TimeZoneInfo.Utc))
                                        .ForJob(job)
                                        .Build();
            _scheduler = _schedulerFactory
                        .GetScheduler(cancellationToken)
                        .GetAwaiter()
                        .GetResult();
            Console.WriteLine("Job scheduling");
            await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            _scheduler.JobFactory = _jobFactory;
            Console.WriteLine("Job starting");
            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler?.Shutdown(cancellationToken);
        }
    }
}