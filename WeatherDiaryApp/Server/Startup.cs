using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api;
using Api.WeatherApi;
using Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Services;
using TimesOfDay = Common.TimesOfDay;

namespace Server
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                               {
                                   options.LoginPath = new PathString("/Account/Login");
                                   options.LogoutPath = new PathString("/Account/Logout");
                               }
                              );
            ConfigureRepository(services, Configuration);
            services.AddScoped<IWeatherApiRequester, WeatherApiApiRequester>();
            services.AddHostedService<WeatherUpdaterHostedService>();
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
            });
        }

        private static IServiceCollection ConfigureRepository(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("debug");
            services.AddScoped<IWeatherDiaryRepository>(s => new WeatherDiaryRepository(connection));
            services.AddDbContext<WeatherDiaryContext>(builder =>
                                                           builder.UseSqlite(connection));
            return services;
        }
    }
}