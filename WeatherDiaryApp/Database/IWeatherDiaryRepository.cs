using System;
using System.Collections.Generic;

namespace Database
{
    public interface IWeatherDiaryRepository
    {
        Common.User AddUser (string email, string password);

        bool ContainsUser (string userEmail);

        List<Common.City> GetAllCities ();

        List<string> GetAllCityNames ();

        Common.City GetCity (string name);

        List<Common.WeatherRecord> GetRecords (string userEmail, string cityName, DateTime time);

        List<string> GetSubscribedCityNamesForUser (string userEmail);

        Common.User GetUser (string userEmail, string password);

        void SaveRecord (Common.WeatherRecord record);

        void StartDiary (string userEmail, string cityName);

        void StopDiary (string userEmail, string cityName);
    }
}