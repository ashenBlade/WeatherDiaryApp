using System.Text.Json.Serialization;

namespace Api.WeatherApi
{
    internal class Current
    {
        /// <summary>
        /// Temperature in Celcius
        /// </summary>
        [JsonPropertyName("temp_c")]
        public double Temperature { get; set; }

        /// <summary>
        /// Speed in kilometers per hour (kmh)
        /// </summary>
        [JsonPropertyName("wind_kph")]
        public double WindSpeed { get; set; }

        /// <summary>
        /// Wind direction in form of "WSW"
        /// </summary>
        [JsonPropertyName("wind_dir")]
        public string WindDirection { get; set; }

        /// <summary>
        /// Overall weather condition
        /// </summary>
        [JsonPropertyName("condition")]
        public Condition Condition { get; set; }

        /// <summary>
        /// Precipitations in millimeters
        /// </summary>
        [JsonPropertyName("precip_mm")]
        public double Precipitations { get; set; }

        /// <summary>
        /// Cloudy in percentage
        /// </summary>
        [JsonPropertyName("cloud")]
        public int Cloudy { get; set; }

        /// <summary>
        /// Pressure in millibars
        /// </summary>
        [JsonPropertyName("pressure_mb")]
        public double Pressure { get; set; }
    }
}