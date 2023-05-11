using System.ComponentModel;

namespace MenuOrder.Shared.Enum
{
    public enum EmailTypeEnum
    {
        [Description("Register Vendor Email Template")]
        RegisterVendor = 1,

        [Description("Welcome Vendor Template")]
        WelcomeVendor = 2
    }
}
