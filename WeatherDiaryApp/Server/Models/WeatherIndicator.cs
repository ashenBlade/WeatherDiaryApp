using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    class WeatherIndicator
    {
        public string City { get; set; }
        public int Temperature { get; set; }
        public enum Precipita
        {

        }
        public enum Phenomena
        {
            
        }
        public int WindSpeed { get; set; }
        public enum WindDirection
        {
            North,
            South,
            East,
            West
        }
        public int Pressure { get; set; }
        public enum Cloudly
        {
            Clearly,
            PartlyCloud,
            CloudCoverWithClarifications,
            Cloudly
        }
        public DateTime Date { get; set; }
        public enum TimesOfDay
        {
            Morning,
            Evening
        }
    }
}
