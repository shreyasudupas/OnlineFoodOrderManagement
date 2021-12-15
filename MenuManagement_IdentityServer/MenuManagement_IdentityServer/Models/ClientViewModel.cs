﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class ClientViewModel : BaseStatusModel
    {
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
    }
}
