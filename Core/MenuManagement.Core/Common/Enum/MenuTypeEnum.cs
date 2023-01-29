using System.ComponentModel;

namespace Inventory.Microservice.Core.Common.Enum
{
    public enum MenuTypeEnum
    {
        [Description("Breakfast")]
        Breakfast,

        [Description("Lunch")]
        Lunch,

        [Description("Dinner")]
        Dinner,

        [Description("Snacks")]
        Snacks

    }
}
