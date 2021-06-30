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

namespace Database.SeedData
{
    internal class Initializer
    {
        public void Seed (WeatherDiaryContext context)
        {
            using (StreamReader reader = new StreamReader(@"../Database/SeedData/city.csv"))
            {
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
        }
    }
}