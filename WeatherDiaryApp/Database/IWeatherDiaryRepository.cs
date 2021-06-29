using System;
using System.Collections.Generic;

namespace Database
{
    public interface IWeatherDiaryRepository
    {
        User AddUser (string email, string password);

        bool ContainsUser (string email);

        City GetCity (string name);

        List<WeatherRecord> GetRecords (User user, City city, DateTime time);

        User GetUser (string email, string password);

        void SaveRecord (WeatherRecord record);

        void StartDiary (User user, City city);

        void StopDiary (User user, City city);
    }
}