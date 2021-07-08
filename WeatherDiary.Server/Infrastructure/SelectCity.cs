using Microsoft.AspNetCore.Mvc;

namespace WeatherDiary.Server.Infrastructure
{
    public class SelectCity
    {
        [FromForm(Name = "Cities")]
        public string Name { get; set; }
    }
}
