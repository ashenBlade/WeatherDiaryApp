using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Server.Infrastructure
{
    public class SelectDiaryOptions
    {
        public SelectDiaryOptions()
        {
        }

        public SelectDiaryOptions(string cityName, bool temperature, bool pressure, bool wind, bool precipitations, bool phenomena, bool cloudy)
        {
            CityName = cityName;
            Temperature = temperature;
            Pressure = pressure;
            Wind = wind;
            Precipitations = precipitations;
            Phenomena = phenomena;
            Cloudy = cloudy;
        }

        [FromForm(Name = "Cities")]
        public string CityName { get; set; }
        public bool Temperature { get; set; }
        public bool Pressure { get; set; }
        public bool Wind { get; set; }
        public bool Precipitations { get; set; }
        public bool Phenomena { get; set; }
        public bool Cloudy { get; set; }
    }
}
