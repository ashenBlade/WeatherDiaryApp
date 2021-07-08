using System.Text.Json.Serialization;

namespace WeatherDiary.Api.WeatherApi
{
    internal class WeatherApiApiResponseRaw
    {
        /// <summary>
        /// Main body of weather status
        /// </summary>
        [JsonPropertyName("current")]
        public Current Current { get; set; }
    }
}