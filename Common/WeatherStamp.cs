namespace Common
{
    public class WeatherStamp
    {
        public WeatherStamp(Cloudy cloudy,
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

        public Cloudy Cloudy { get; private set; }
        public Phenomena Phenomena { get; private set; }
        public Precipitation Precipitation { get; private set; }
        public int Pressure { get; private set; }
        public int Temperature { get; private set; }
        public WindDirection WindDirection { get; private set; }
        public int WindSpeed { get; private set; }
    }
}