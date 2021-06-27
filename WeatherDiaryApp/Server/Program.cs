using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherDiary.Data;
using Server.Models;
using System;
using Server;

namespace MvcMovie
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder (string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void Main (string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.Initialize(services);
                    var context = services.GetRequiredService<WeatherDiaryContext>();
                    IWeatherDiaryRepository repository = new WeatherDiaryRepository(context);
                    repository.Register("@", "666");
                    var user = repository.GetUser("@", "666");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }
    }
}