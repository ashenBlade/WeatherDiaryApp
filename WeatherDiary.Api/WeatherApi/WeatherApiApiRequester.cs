using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherDiary.Domain;

namespace WeatherDiary.Api.WeatherApi
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

        private static Uri GetRequestUri(string city) =>
            new Uri(GetRequestUrl(city));

        public WeatherStamp GetRecord(string city)
        {
            var url = GetRequestUrl(city);
            string responseJson;
            try
            {
                responseJson = _client.GetStringAsync(url)
                                          .GetAwaiter()
                                          .GetResult();
            }
            catch (Exception e)
            {
                return EmptyWeatherApiRequester.EmptyStamp;
            }
            var responseRaw = JsonSerializer.Deserialize<WeatherApiApiResponseRaw>(responseJson);
            var record = WeatherApiRawResponseConverter.Convert(responseRaw);
            return record;
        }

        public Task<WeatherStamp> GetRecordTask(string city)
        {
            throw new System.NotImplementedException();
        }
    }
}