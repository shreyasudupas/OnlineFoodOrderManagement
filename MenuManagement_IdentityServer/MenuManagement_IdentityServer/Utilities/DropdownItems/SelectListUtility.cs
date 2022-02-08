using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Utilities.DropdownItems
{
    public class SelectListUtility
    {
        public static List<SelectListItem> GetCityItems()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Bangalore", Value = "Bangalore" },
                new SelectListItem { Text = "Mumbai", Value = "Mumbai" },
                new SelectListItem { Text = "Chennai", Value = "Chennai" },
                new SelectListItem { Text = "Hydrabad", Value = "Hydrabad" }
            };
        }

        public static List<SelectListItem> GetStateItems()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Karnataka", Value = "Karnataka" },
                new SelectListItem { Text = "Maharastra", Value = "Maharastra" },
                new SelectListItem { Text = "Tamil Nadu", Value = "Tamil Nadu" },
                new SelectListItem { Text = "Telangana", Value = "Telangana" }
            };
        }
    }
}
