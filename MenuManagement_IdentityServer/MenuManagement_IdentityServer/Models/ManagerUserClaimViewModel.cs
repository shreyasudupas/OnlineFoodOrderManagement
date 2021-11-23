using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class ManagerUserClaimViewModel : BaseStatusModel
    {
        public Dictionary<string, string> UserClaimsSelectOptionList { get; set; }
        public string UserClaimValue { get; set; }
        public string UserId { get; set; }
    }
}
