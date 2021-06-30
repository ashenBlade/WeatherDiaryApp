using System.Threading.Tasks;
using Common;

namespace Api.WeatherApi
{
    public class WeatherApiApiRequester : IWeatherApiRequester
    {
        public WeatherIndicator GetRecord(string city)
        {
            throw new System.NotImplementedException();
        }

        public Task<WeatherIndicator> GetRecordTask(string city)
        {
            throw new System.NotImplementedException();
        }
    }
}