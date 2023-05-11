using MenuManagement_IdentityServer.Data.Models;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class UserDashboard : BaseStatusModel
    {
        public UserDashboard()
        {
            Roles = new List<string>();
            Claims = new List<UserClaim>();
        }
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
        public List<UserClaim> Claims { get; set; }
    }

    public class UserClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
