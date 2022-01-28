using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class ManagerUserClaimViewModelPost
    {
        [Required]
        public string UserId { get; set; }
        //Selected Claim Item
        [Required]
        public string ClaimTypeSelected { get; set; }
        //[Required]
        //public string UserClaimValue { get; set; }

    }
}
