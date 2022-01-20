using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class AddCorsAllowedOriginViewModel : BaseStatusModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientOriginUrl { get; set; }
    }
}
