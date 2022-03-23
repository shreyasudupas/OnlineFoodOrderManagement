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

        //
        public static List<SelectListItem> GetGrantTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value="authorization_code" ,Text="Authorization Code" }
            };
        }

        //This will be shown in access_token
        public static List<SelectListItem> GetAllowedScopeList()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Value = "userRole", Text = "Role" },
                new SelectListItem { Value = "office", Text = "Office" },
                new SelectListItem { Value = "profile", Text = "Profile" },
                new SelectListItem { Value = "openid", Text = "OpenId" },
            };
        }
    }
}
