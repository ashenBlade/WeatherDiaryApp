using System.Collections.Generic;
using Server.Models;
using System;

namespace WeatherDiary.Data
{
    public interface IWeatherDiaryRepository
    {
        bool ContainsUser (string email);

        List<City> GetAllCitiesForUser (User user);

        List<City> GetLocationsForRecords ();

        List<WeatherRecord> GetRecords (User user, City city, DateTime time);

        User GetUser (string email, string password);

        User Register (string email, string password);

        void SaveRecord (WeatherRecord record);

        void StartDiary (User user, City city);

        void StopDiary (User user, City city);
    }
}