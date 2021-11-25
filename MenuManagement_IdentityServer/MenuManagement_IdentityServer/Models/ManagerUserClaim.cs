using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class ManagerUserClaim
    {
        //public Dictionary<string, string> UserClaims { get; set; }
        //public ManagerUserClaim()
        //{
        //    UserClaims = new List<SelectListItem>();
        //}
        //public List<SelectListItem> UserClaims { get; set; }
        public SelectList UserClaims { get; set; }
        public string ErrorDescription { get; set; }
        public CrudEnumStatus status { get; set; }
    }

    public class ClaimModel
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
        
}
