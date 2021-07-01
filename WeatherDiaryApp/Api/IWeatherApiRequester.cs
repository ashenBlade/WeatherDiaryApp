using System.Threading.Tasks;
using Common;

namespace Api
{
    public interface IWeatherApiRequester
    {
        WeatherIndicator GetRecord(string city);
        Task<WeatherIndicator> GetRecordTask(string city);
    }
}