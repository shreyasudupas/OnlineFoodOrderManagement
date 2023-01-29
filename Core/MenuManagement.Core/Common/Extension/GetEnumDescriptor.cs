using Inventory.Microservice.Core.Common.Enum;
using System;
using System.ComponentModel;

namespace Inventory.Microservice.Core.Common.Extension
{
    public static class Extension
    {
        public static string GetEnumDescription(this MenuTypeEnum menuTypeValue)
        {
            var field = menuTypeValue.GetType().GetField(menuTypeValue.ToString());
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            throw new ArgumentException("Item not found.", nameof(menuTypeValue));
        }
    }
}
