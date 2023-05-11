using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<UserAddress> Address { get; set; }
        public bool IsAdmin { get; set; }
        public string ImagePath { get; set; }
        public int CartAmount { get; set; }
        public double Points { get; set; }
    }
}
