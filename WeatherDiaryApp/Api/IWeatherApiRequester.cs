using System.Threading.Tasks;
using Common;

namespace Api
{
    public interface IWeatherApiRequester
    {
        WeatherStamp GetRecord(string city);
        Task<WeatherStamp> GetRecordTask(string city);
    }
}