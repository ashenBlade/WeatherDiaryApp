using Common;

namespace Api.WeatherApi
{
    internal static class WeatherApiRawResponseConverter
    {
        internal static WeatherIndicator Convert(WeatherApiApiResponseRaw response)
        {
            return new WeatherIndicator();
        }
    }
}