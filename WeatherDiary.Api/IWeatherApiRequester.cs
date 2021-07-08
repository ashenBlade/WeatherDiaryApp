using System.Threading.Tasks;
using WeatherDiary.Domain;

namespace WeatherDiary.Api
{
    public interface IWeatherApiRequester
    {
        WeatherStamp GetRecord(string city);
        Task<WeatherStamp> GetRecordTask(string city);
    }
}