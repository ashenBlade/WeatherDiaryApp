using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common;

namespace Api.WeatherApi
{
    public class WeatherApiApiRequester : IWeatherApiRequester
    {
        private const string _baseUrl = "http://api.weatherapi.com/v1/current.json";
        private const string _apiKey = "9429cccf088748ff84b55742212706";
        private HttpClient _client;

        public WeatherApiApiRequester()
        {
            _client = new HttpClient();
        }

        private static string GetRequestUrl(string city) =>
            $"{_baseUrl}?q={city}&key={_apiKey}";

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