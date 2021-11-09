using Microsoft.AspNetCore.Identity;
using System;

namespace MenuManagement_IdentityServer.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public string City { get; set; }
        public bool IsAdmin { get; set; }
    }
}
