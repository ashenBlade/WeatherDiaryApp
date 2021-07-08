using System.ComponentModel;

namespace WeatherDiary.Domain
{
    public enum WindDirection
    {
        None,
        [Description("С")]
        N,
        [Description("СЗ")]
        NW,
        [Description("СВ")]
        NE,
        [Description("Ю")]
        S,
        [Description("ЮЗ")]
        SW,
        [Description("ЮВ")]
        SE,
        [Description("З")]
        W,
        [Description("В")]
        E
    }
}