﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class ClientViewModel : BaseStatusModel
    {
        public ClientViewModel()
        {
            RedirectUrls = new Dictionary<int, string>();
            AllowedCorsOrigins = new Dictionary<int, string>();
            PostRedirectUrls = new Dictionary<int, string>();
            ClientSecrets = new Dictionary<int, string>();
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

        public Dictionary<int, string> RedirectUrls { get; set; }
        public Dictionary<int, string> AllowedCorsOrigins { get; set; }
        public Dictionary<int, string> PostRedirectUrls { get; set; }
        public Dictionary<int,string> ClientSecrets { get; set; }
    }
}
