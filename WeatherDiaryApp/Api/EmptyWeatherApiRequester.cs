using System.Threading.Tasks;
using Common;

namespace Api
{
    public class EmptyWeatherApiRequester : IWeatherApiRequester
    {
        public WeatherIndicator GetRecord(string city)
        {
            // Empty
            return new WeatherIndicator();
        }

        public Task<WeatherIndicator> GetRecordTask(string city)
        {
            // Empty
            return new Task<WeatherIndicator>(() => new WeatherIndicator());
        }
    }
}