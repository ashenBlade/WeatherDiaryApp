using System;
using System.Collections.Generic;

namespace WeatherDiary.Data
{
    public interface IWeatherDiaryRepository
    {
        Domain.User AddUser (string email, string password);

        bool ContainsUser (string userEmail);

        List<Domain.City> GetAllCities ();

        List<string> GetAllCityNames ();

        Domain.City GetCity (string name);

        List<Domain.WeatherRecord> GetRecords (string userEmail, string cityName, DateTime time);

        List<Domain.WeatherRecord> GetRecords (string userEmail, string cityName);

        List<string> GetSubscribedCityNamesForUser (string userEmail);

        Domain.User GetUser (string userEmail, string password);

        void SaveRecord (Domain.WeatherRecord record);

        void StartDiary (string userEmail, string cityName);

        void StopDiary (string userEmail, string cityName);
    }
}