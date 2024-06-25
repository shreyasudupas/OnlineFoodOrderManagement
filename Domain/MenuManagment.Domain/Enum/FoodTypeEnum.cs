using System.ComponentModel;

namespace MenuManagment.Mongo.Domain.Enum
{
    public enum FoodTypeEnum
    {
        [Description("Vegetarian")]
        Vegetarian,
        [Description("Non Vegetarian")]
        NonVegetarian,
        [Description("Eggetarian")]
        Eggetarian,
        [Description("Vegan")]
        Vegan,
    }
}
