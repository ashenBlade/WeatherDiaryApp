using System.Text.Json.Serialization;

namespace WeatherDiary.Api.WeatherApi
{
    internal class Condition
    {
        /// <summary>
        /// Short weather description summary
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// Code of the weather condition
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}