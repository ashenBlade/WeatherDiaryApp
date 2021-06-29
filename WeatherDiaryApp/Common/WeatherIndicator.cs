namespace Common
{
    public class WeatherIndicator
    {
        public WeatherIndicator() { }
        public WeatherIndicator(Cloudy cloudy, 
            Phenomena phenomena, 
            Precipitation precipitation, 
            int pressure, 
            int temperature, 
            WindDirection windDirection, 
            int windSpeed)
        {
            Cloudy = cloudy;
            Phenomena = phenomena;
            Precipitation = precipitation;
            Pressure = pressure;
            Temperature = temperature;
            WindDirection = windDirection;
            WindSpeed = windSpeed;
        }

        public Cloudy Cloudy { get; set; }
        public Phenomena Phenomena { get; set; }
        public Precipitation Precipitation { get; set; }
        public int Pressure { get; set; }
        public int Temperature { get; set; }
        public WindDirection WindDirection { get; set; }
        public int WindSpeed { get; set; }
    }
}