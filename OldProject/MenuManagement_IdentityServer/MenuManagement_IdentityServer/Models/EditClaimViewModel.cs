using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class EditClaimViewModel : BaseStatusModel
    {
        public int? ClaimId { get; set; }
        [Required]
        public string ClaimType { get; set; }
        [Required]
        public string ClaimValue { get; set; }
    }
}
