using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherDiary.Api;
using WeatherDiary.Api.WeatherApi;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WeatherDiary.Data;
using WeatherDiary.Server.Services;
using TimesOfDay = WeatherDiary.Domain.TimesOfDay;

namespace WeatherDiary.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            ConfigureLogging(services, Configuration);

            ConfigureAuthentication(services, Configuration);

            // Storing of weather and user data
            ConfigureRepository(services, Configuration);

            // Weather retrieve from server
            ConfigureBackgroundTasks(services, Configuration);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Diary/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                                             name: "default",
                                             pattern: "{controller=Diary}/{action=Select}/{id?}");
                endpoints.MapFallback(pattern: "/",
                                      async context => await context.Response.WriteAsync("Fallback"));
            });
        }

        private static IServiceCollection ConfigureLogging(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole()
                       .AddConfiguration(configuration);
            });
            return services;
        }

        private static IServiceCollection ConfigureAuthentication(IServiceCollection services,
                                                                  IConfiguration configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                                .AddCookie(options =>
                                           {
                                               options.LoginPath = new PathString("/Account/Login");
                                               options.LogoutPath = new PathString("/Account/Logout");
                                           }
                                          );
            return services;
        }

        private static IServiceCollection ConfigureRepository(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("debug");
            services.AddTransient<IWeatherDiaryRepository>(s => new WeatherDiaryRepository(connection));
            services.AddDbContext<WeatherDiaryContext>(builder =>
                                                           builder.UseSqlite(connection));
            return services;
        }

        private static IServiceCollection ConfigureBackgroundTasks(IServiceCollection services,
                                                                   IConfiguration configuration)
        {
            ConfigureQuartzHostedService(services, configuration);
            services.AddHostedService<QuartzWeatherUpdaterHostedService>();
            return services;
        }

        private static void ConfigureQuartzHostedService(IServiceCollection services, IConfiguration configuration)
        {
#if DEBUG
            services.AddTransient<IWeatherApiRequester, EmptyWeatherApiRequester>();
#else
            services.AddTransient<IWeatherApiRequester, WeatherApiApiRequester>();
#endif
            services.AddTransient<WeatherRetrieveJob>();

            // Configuring Quartz.NET
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, MicrosoftDIJobFactory>();
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.UseInMemoryStore();
            });
            services.AddQuartzHostedService(config => config.WaitForJobsToComplete = true);
        }
    }
}