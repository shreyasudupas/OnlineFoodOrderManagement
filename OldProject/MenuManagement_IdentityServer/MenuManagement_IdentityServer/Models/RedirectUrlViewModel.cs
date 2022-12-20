using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class RedirectUrlViewModel  : BaseStatusModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientRedirectUrl { get; set; }
    }
}
