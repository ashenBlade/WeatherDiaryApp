using System;
using System.Collections.Generic;

namespace Database
{
    public interface IWeatherDiaryRepository
    {
        User AddUser (string email, string password);

        bool ContainsUser (string userEmail);

        List<City> GetAllCities ();

        City GetCity (string name);

        List<WeatherRecord> GetRecords (string userEmail, string cityName, DateTime time);

        User GetUser (string userEmail, string password);

        void SaveRecord (WeatherRecord record);

        void StartDiary (string userEmail, string cityName);

        void StopDiary (string userEmail, string cityName);
    }
}