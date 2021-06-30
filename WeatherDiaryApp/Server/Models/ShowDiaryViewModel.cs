using System;
using Common;
using System.Collections.Generic;
using System.Linq;
namespace Server.Models
{
    public class ShowDiaryViewModel
    {
        public ShowDiaryViewModel() { }

        public ShowDiaryViewModel(IEnumerable<WeatherRecord> weatherRecords)
        {
            Records = weatherRecords ?? throw new ArgumentNullException(nameof(weatherRecords));
        }

        public IEnumerable<WeatherRecord> Records { get; set; }
    }
}
