using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Quartz;

namespace WeatherDiary.Server.Services
{
    public class SimpleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Hello");
        }
    }
}