using Common;
using System;
using System.Collections.Generic;

namespace Api.WeatherApi
{
    internal static class WeatherApiRawResponseConverter
    {
        internal static WeatherIndicator Convert(WeatherApiApiResponseRaw response)
        {
            var ConvertedIndicator = new WeatherIndicator();
            ConvertedIndicator.Pressure = (int)response.Current.Pressure;
            ConvertedIndicator.Temperature = (int)response.Current.Temperature;
            ConvertedIndicator.WindSpeed = (int)response.Current.WindSpeed;
            ConvertedIndicator.Cloudy = ConvertCloudy(response);
            ConvertedIndicator.WindDirection = ConvertWindDirection(response);          
            ConvertedIndicator = ConvertPrecipitations(ConvertedIndicator, response);           
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
            return (WindDirection)Enum.Parse(typeof(WindDirection), response.Current.WindDirection);
        }

        private static WeatherIndicator ConvertPrecipitations
            (WeatherIndicator ConvertedIndicator, WeatherApiApiResponseRaw response)
        {
            var WeatherConvertedIndicator = new WeatherIndicator();
            var PrecipitationsDictionary = InitializeDictionary();
            WeatherConvertedIndicator.Phenomena = PrecipitationsDictionary[response.Current.Condition.Code].Item1;
            WeatherConvertedIndicator.Precipitation = PrecipitationsDictionary[response.Current.Condition.Code].Item2;
            return WeatherConvertedIndicator;
        }

        private static  Dictionary<int, Tuple<Phenomena, Precipitation>> InitializeDictionary()
        {
            var PrecipitationsDictionary = new Dictionary<int, Tuple<Phenomena, Precipitation>>();
            PrecipitationsDictionary.Add(1000, Tuple.Create(Phenomena.None, Precipitation.None));
            PrecipitationsDictionary.Add(1003, Tuple.Create(Phenomena.None, Precipitation.None));
            PrecipitationsDictionary.Add(1006, Tuple.Create(Phenomena.None, Precipitation.None));
            PrecipitationsDictionary.Add(1009, Tuple.Create(Phenomena.None, Precipitation.None));
            PrecipitationsDictionary.Add(1030, Tuple.Create(Phenomena.None, Precipitation.None));
            PrecipitationsDictionary.Add(1063, Tuple.Create(Phenomena.None, Precipitation.Rain));
            PrecipitationsDictionary.Add(1066, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1069, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1072, Tuple.Create(Phenomena.None, Precipitation.Drizzle));
            PrecipitationsDictionary.Add(1087, Tuple.Create(Phenomena.Thunderstorm, Precipitation.None));
            PrecipitationsDictionary.Add(1114, Tuple.Create(Phenomena.Snowstorm, Precipitation.None));
            PrecipitationsDictionary.Add(1117, Tuple.Create(Phenomena.Snowstorm, Precipitation.Snow));
            PrecipitationsDictionary.Add(1135, Tuple.Create(Phenomena.Fog, Precipitation.None));
            PrecipitationsDictionary.Add(1147, Tuple.Create(Phenomena.Fog, Precipitation.Drizzle));
            PrecipitationsDictionary.Add(1150, Tuple.Create(Phenomena.None, Precipitation.Drizzle));
            PrecipitationsDictionary.Add(1153, Tuple.Create(Phenomena.None, Precipitation.Drizzle));
            PrecipitationsDictionary.Add(1168, Tuple.Create(Phenomena.Hoarfrost, Precipitation.Drizzle));
            PrecipitationsDictionary.Add(1171, Tuple.Create(Phenomena.Ice, Precipitation.Drizzle));
            PrecipitationsDictionary.Add(1180, Tuple.Create(Phenomena.None, Precipitation.Rain));
            PrecipitationsDictionary.Add(1183, Tuple.Create(Phenomena.None, Precipitation.Rain));
            PrecipitationsDictionary.Add(1186, Tuple.Create(Phenomena.None, Precipitation.Rain));
            PrecipitationsDictionary.Add(1192, Tuple.Create(Phenomena.None, Precipitation.Hail));
            PrecipitationsDictionary.Add(1195, Tuple.Create(Phenomena.Fog, Precipitation.Hail));
            PrecipitationsDictionary.Add(1198, Tuple.Create(Phenomena.Hoarfrost, Precipitation.Rain));
            PrecipitationsDictionary.Add(1201, Tuple.Create(Phenomena.Hoarfrost, Precipitation.Rain));
            PrecipitationsDictionary.Add(1204, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1207, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1210, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1213, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1216, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1219, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1222, Tuple.Create(Phenomena.Snowstorm, Precipitation.Snow));
            PrecipitationsDictionary.Add(1225, Tuple.Create(Phenomena.Snowstorm, Precipitation.Snow));
            PrecipitationsDictionary.Add(1237, Tuple.Create(Phenomena.Ice, Precipitation.Snow));
            PrecipitationsDictionary.Add(1240, Tuple.Create(Phenomena.None, Precipitation.Rain));
            PrecipitationsDictionary.Add(1243, Tuple.Create(Phenomena.None, Precipitation.Rain));
            PrecipitationsDictionary.Add(1246, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Rain));
            PrecipitationsDictionary.Add(1243, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1252, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1255, Tuple.Create(Phenomena.None, Precipitation.Snow));
            PrecipitationsDictionary.Add(1258, Tuple.Create(Phenomena.None, Precipitation.SnowGroats));
            PrecipitationsDictionary.Add(1261, Tuple.Create(Phenomena.Ice, Precipitation.Snow));
            PrecipitationsDictionary.Add(1264, Tuple.Create(Phenomena.Ice, Precipitation.SnowGroats));
            PrecipitationsDictionary.Add(1273, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Rain));
            PrecipitationsDictionary.Add(1276, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Rain));
            PrecipitationsDictionary.Add(1279, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Snow));
            PrecipitationsDictionary.Add(1279, Tuple.Create(Phenomena.Thunderstorm, Precipitation.Snow));
            return PrecipitationsDictionary;
        }
        
    }
}