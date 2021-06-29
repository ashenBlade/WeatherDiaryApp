using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Models;
using System;
using Server;

namespace MvcMovie
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder (string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        public static void Main (string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}