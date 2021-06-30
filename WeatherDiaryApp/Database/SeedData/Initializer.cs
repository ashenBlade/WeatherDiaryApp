using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

namespace Database.SeedData
{
    internal class Initializer
    {
        public void SeedCity (WeatherDiaryContext context)
        {
            using StreamReader reader = new StreamReader(@"../Database/SeedData/city.csv");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HasHeaderRecord = false,
            };
            CsvReader csvReader = new CsvReader(reader, config);
            var cities = csvReader.GetRecords<City>().ToArray();
            context.Cities.AddRange(cities);
            context.SaveChanges();
        }

        public void SeedWeather (WeatherDiaryContext context)
        {
            using StreamReader reader = new StreamReader(@"../Database/SeedData/records.csv");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
            };
            CsvReader csvReader = new CsvReader(reader, config);
            var records = csvReader.GetRecords<WeatherRecord>().ToArray();
            Array.ForEach(records, r => r.City = null);
            context.WeatherRecords.AddRange(records);
            context.SaveChanges();
        }
    }
}