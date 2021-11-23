using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class ManagerUserClaim
    {
        public Dictionary<string, string> UserClaims { get; set; }
        public string ErrorDescription { get; set; }
        public CrudEnumStatus status { get; set; }
    }
}
