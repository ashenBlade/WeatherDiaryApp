using System.ComponentModel;

namespace Common
{
    public enum WindDirection
    {
        [Description("С")]
        N,
        [Description("СВ")]
        NW,
        [Description("СЗ")]
        NE,
        [Description("Ю")]
        S,
        [Description("ЮВ")]
        SW,
        [Description("ЮЗ")]
        SE,
        [Description("В")]
        W,
        [Description("З")]
        E
    }
}
