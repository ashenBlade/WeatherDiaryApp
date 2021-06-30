using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Common;
using Server.Infrastructure;

namespace Server.Models
{
    public class ShowDiaryViewModel
    {
        public ShowDiaryViewModel () { }

        public ShowDiaryViewModel(string userEmail, Database.IWeatherDiaryRepository repository, SelectDiaryOptions options)
        {
            Records = repository.GetRecords(userEmail, options.CityName, DateTime.Now);
        }

        public IEnumerable<WeatherRecord> Records { get; set; }
        public SelectDiaryOptions Options { get; set; }
        public int NumOfOptions
        {
            get { var num = 0;
                if (Options.Temperature) num++;
                if (Options.Pressure) num++;
                if (Options.Precipitations) num++;
                if (Options.Phenomena) num++;
                if (Options.Cloudy) num++;
                if (Options.Wind) num++;
                return num;
            }
        }
    }
}
