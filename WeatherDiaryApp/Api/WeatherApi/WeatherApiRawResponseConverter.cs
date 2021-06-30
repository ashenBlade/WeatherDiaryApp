using Common;
using System;
using System.Collections.Generic;

namespace Api.WeatherApi
{
    internal static class WeatherApiRawResponseConverter
    {
        static Dictionary<int, Tuple<Phenomena, Precipitation>> CodeToPhenomenaAndPrecipitationsDictionary;
        static WeatherApiRawResponseConverter()
        {
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1000, Tuple.Create(Phenomena.None, Precipitation.None));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1003, Tuple.Create(Phenomena.None, Precipitation.None));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1006, Tuple.Create(Phenomena.None, Precipitation.None));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1009, Tuple.Create(Phenomena.None, Precipitation.None));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1030, Tuple.Create(Phenomena.None, Precipitation.None));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1063, Tuple.Create(Phenomena.None, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1066, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1069, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1072, Tuple.Create(Phenomena.None, Precipitation.Drizzle));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1087, Tuple.Create(Phenomena.Thunderstorm, Precipitation.None));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1114, Tuple.Create(Phenomena.Snowstorm, Precipitation.None));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1117, Tuple.Create(Phenomena.Snowstorm, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1135, Tuple.Create(Phenomena.Fog, Precipitation.None));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1147, Tuple.Create(Phenomena.Fog, Precipitation.Drizzle));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1150, Tuple.Create(Phenomena.None, Precipitation.Drizzle));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1153, Tuple.Create(Phenomena.None, Precipitation.Drizzle));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1168, Tuple.Create(Phenomena.Hoarfrost, Precipitation.Drizzle));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1171, Tuple.Create(Phenomena.Ice, Precipitation.Drizzle));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1180, Tuple.Create(Phenomena.None, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1183, Tuple.Create(Phenomena.None, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1186, Tuple.Create(Phenomena.None, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1192, Tuple.Create(Phenomena.None, Precipitation.Hail));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1195, Tuple.Create(Phenomena.Fog, Precipitation.Hail));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1198, Tuple.Create(Phenomena.Hoarfrost, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1201, Tuple.Create(Phenomena.Hoarfrost, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1204, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1207, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1210, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1213, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1216, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1219, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1222, Tuple.Create(Phenomena.Snowstorm, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1225, Tuple.Create(Phenomena.Snowstorm, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1237, Tuple.Create(Phenomena.Ice, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1240, Tuple.Create(Phenomena.None, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1243, Tuple.Create(Phenomena.None, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1246, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1243, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1252, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1255, Tuple.Create(Phenomena.None, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1258, Tuple.Create(Phenomena.None, Precipitation.SnowGroats));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1261, Tuple.Create(Phenomena.Ice, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1264, Tuple.Create(Phenomena.Ice, Precipitation.SnowGroats));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1273, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1276, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Rain));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1279, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Snow));
            CodeToPhenomenaAndPrecipitationsDictionary.Add(1279, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Snow));
        }

        internal static WeatherIndicator Convert(WeatherApiApiResponseRaw response)
        {
            var ConvertedIndicator = new WeatherIndicator();
            ConvertedIndicator.Pressure = (int)response.Current.Pressure;
            ConvertedIndicator.Temperature = (int)response.Current.Temperature;
            ConvertedIndicator.WindSpeed = (int)response.Current.WindSpeed;
            ConvertedIndicator.Cloudy = ConvertCloudy(response);
            ConvertedIndicator.WindDirection = ConvertWindDirection(response);
            ConvertedIndicator.Precipitation = ConvertPrecipitations(response);
            ConvertedIndicator.Phenomena = ConvertPhenomena(response);
            return ConvertedIndicator;
        }

        private static Cloudy ConvertCloudy(WeatherApiApiResponseRaw response)
        {
            if (response.Current.Cloudy < 15)
                return Cloudy.Cloudless;
            if (response.Current.Cloudy < 40)
                return Cloudy.PartlyCloud;
            if (response.Current.Cloudy < 70)
                return Cloudy.AverageCloud;
            else return Cloudy.SolidCloud;
        }

        private static WindDirection ConvertWindDirection(WeatherApiApiResponseRaw response)
        {
            if (response.Current.WindDirection.Length <= 2)
                return (WindDirection)Enum.Parse(typeof(WindDirection), response.Current.WindDirection);
            else return (WindDirection)Enum.Parse(typeof(WindDirection), response.Current.WindDirection.Substring(1));
        }

        private static Precipitation ConvertPrecipitations(WeatherApiApiResponseRaw response)
        {
            return CodeToPhenomenaAndPrecipitationsDictionary[response.Current.Condition.Code].Item2;
        }

        private static Phenomena ConvertPhenomena(WeatherApiApiResponseRaw response)
        {
            return CodeToPhenomenaAndPrecipitationsDictionary[response.Current.Condition.Code].Item1;
        }

    }
}