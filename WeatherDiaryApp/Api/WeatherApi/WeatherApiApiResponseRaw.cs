using System.Text.Json.Serialization;

namespace Api.WeatherApi
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