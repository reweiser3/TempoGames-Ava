using System.ComponentModel;

namespace Ava.Data.Users
{
    public enum Country
    {
        [Description("United States")]
        UnitedStates,
        [Description("China")]
        China,
        [Description("Japan")]
        Japan,
        [Description("South Korea")]
        SouthKorea,
        [Description("Germany")]
        Germany,
        [Description("United Kingdom")]
        UnitedKingdom,
        [Description("Brazil")]
        Brazil,
        [Description("Canada")]
        Canada,
        [Description("France")]
        France,
        [Description("Other")]
        Other
    }
}