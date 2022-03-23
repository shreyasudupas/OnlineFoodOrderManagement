using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class DeleteUserClaimGet : BaseStatusModel
    {
        public DeleteUserClaimGet()
        {
            UserClaims = new List<UserClaimList>();
        }
        public string UserId { get; set; }
        public List<UserClaimList> UserClaims { get; set; }
    }

    public class UserClaimList
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public bool IsDeleteSelection { get; set; }
    }
}
