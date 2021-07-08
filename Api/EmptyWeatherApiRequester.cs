using System.Threading.Tasks;
using Common;

namespace Api
{
    public class EmptyWeatherApiRequester : IWeatherApiRequester
    {
        internal static readonly WeatherStamp EmptyStamp = new WeatherStamp(Cloudy.Cloudless,
                                                                  Phenomena.None,
                                                                  Precipitation.None,
                                                                  0,
                                                                  0,
                                                                  WindDirection.None,
                                                                  0);
        public WeatherStamp GetRecord(string city)
        {
            // Empty
            return EmptyStamp;
        }

        public Task<WeatherStamp> GetRecordTask(string city)
        {
            // Empty
            return new Task<WeatherStamp>(() => EmptyStamp);
        }
    }
}