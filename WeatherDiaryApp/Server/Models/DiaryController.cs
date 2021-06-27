namespace Server.Models
{
    public class DiaryController
    {
        public void SubscribeWeatherDiary(string currentCity)
        {
            //метод подписки на дневник получает город и передаёт его парсеру для начала
            //отслеживания погоды в этом городе
        }

        public WeatherIndicator GetWeatherDiary(string currentCity, TimesOfDay timeOfDay)
        {
            //в методе производится отправка запроса в бд по городу и времени суток
            //бд отправляет составляющие WeatherIndicator и происходит формирование WeatherIndicator и отправка
            return null;
        }

        public void UnsubscribeWeatherDiary(string currentCity)
        {
            //метод отписки от дневника получает город и передаёт его парсеру для остановки
            //отслеживания погоды в этом городе
        }
    }
}
