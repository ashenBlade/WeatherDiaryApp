using Microsoft.AspNetCore.Mvc;

namespace Server.Infrastructure
{
    public class SelectDiaryOptions
    {
        [FromForm(Name = "Cities")]
        public string CityName { get; set; }
        public bool Temperature { get; set; }
        public bool Pressure { get; set; }
        public bool Wind { get; set; }
        public bool Precipitations { get; set; }
        public bool Cloudy { get; set; }
    }
}