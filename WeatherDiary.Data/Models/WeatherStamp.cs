namespace WeatherDiary.Data.Models
{
    public class WeatherStamp
    {
        public int Id { get; set; }
        /// <summary>
        /// Atmosphere pressure in millimeters of mercury
        /// </summary>
        public int Pressure { get; set; }
        /// <summary>
        /// Temperature in Celsius
        /// </summary>
        public int Temperature { get; set; }
        /// <summary>
        /// Wind speed in m/s
        /// </summary>
        public int WindSpeed { get; set; }
        public WindDirection WindDirection { get; set; }
        public Cloudy Cloudy { get; set; }
        public Phenomena Phenomena { get; set; }
        public Precipitation Precipitation { get; set; }
    }
}