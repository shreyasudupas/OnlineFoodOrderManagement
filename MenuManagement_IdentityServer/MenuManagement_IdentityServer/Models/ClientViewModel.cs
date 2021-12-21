using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class ClientViewModel : BaseStatusModel
    {
        public ClientViewModel()
        {
            RedirectUrls = new List<string>();
            AllowedCorsOrigins = new List<string>();
            PostRedirectUrls = new List<string>();
        }
        [Required]
        public string ClientId { get; set; }
        public bool RequireClientSecret { get; set; }
        [Required]
        public string ClientName { get; set; }
        public string Description { get; set; }
        public bool RequireConsent { get; set; }
        [Required]
        public int AccessTokenLifetime { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<string> RedirectUrls { get; set; }
        public List<string> AllowedCorsOrigins { get; set; }
        public List<string> PostRedirectUrls { get; set; }
    }
}
