using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class ClientSecretViewModel : BaseStatusModel
    {
        public string ClientId { get; set; }
        [Required]
        public string ClientSecret { get; set; }
        public string Description { get; set; }
        public string ExpirationDate { get; set; }
    }
}
