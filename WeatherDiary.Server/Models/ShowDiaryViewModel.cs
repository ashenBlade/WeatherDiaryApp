using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using WeatherDiary.Domain;
using WeatherDiary.Data;
using WeatherDiary.Server.Infrastructure;

namespace WeatherDiary.Server.Models
{
    public class ShowDiaryViewModel
    {
        public ShowDiaryViewModel () { }

        public ShowDiaryViewModel(string userEmail, IWeatherDiaryRepository repository, SelectDiaryOptions options)
        {
            Records = repository.GetRecords(userEmail, options.CityName, DateTime.Now);
            Options = options;
        }

        public IEnumerable<WeatherRecord> Records { get; set; }
        public SelectDiaryOptions Options { get; set; }
        public int NumOfOptions
        {
            get { var num = 0;
                if (Options.Temperature) num++;
                if (Options.Pressure) num++;
                if (Options.Precipitations) num++;
                if (Options.Phenomena) num++;
                if (Options.Cloudy) num++;
                if (Options.Wind) num++;
                return num;
            }
        }

        /// <summary>
        /// Приведение значения перечисления в удобочитаемый формат.
        /// </summary>
        /// <remarks>
        /// Для корректной работы необходимо использовать атрибут [Description("NameRu")] для каждого элемента перечисления.
        /// </remarks>
        /// <param name="enumElement">Элемент перечисления</param>
        /// <returns>Название элемента</returns>
        public string GetDescription(Enum enumElement)
        {
            Type type = enumElement.GetType();

            MemberInfo[] memInfo = type.GetMember(enumElement.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumElement.ToString();
        }
    }
}
