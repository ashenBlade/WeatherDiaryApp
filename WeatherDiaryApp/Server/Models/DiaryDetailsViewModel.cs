using System;
using Common;
using System.Collections.Generic;
using System.Linq;
namespace Server.Models
{
    public class DiaryDetailsViewModel
    {
        public DiaryDetailsViewModel(IEnumerable<WeatherRecord> weatherRecords)
        {
            Records = weatherRecords ?? throw new ArgumentNullException(nameof(weatherRecords));
        }

        public IEnumerable<WeatherRecord> Records { get; set; }
    }
}
