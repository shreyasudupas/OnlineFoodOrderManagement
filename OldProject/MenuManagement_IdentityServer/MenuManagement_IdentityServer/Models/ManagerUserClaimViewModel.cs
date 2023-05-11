using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class ManagerUserClaimViewModel : BaseStatusModel
    {
        public SelectList SelectionUserClaims { get; set; }
        public string ClaimTypeSelected { get; set; }
        public string UserClaimValue { get; set; }
        public string UserId { get; set; }
    }
}
