using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityServer.Infrastruture.Database
{
    public class ApplicationUser : IdentityUser
    {
        public List<UserAddress> Address { get; set; }
        //public bool IsAdmin { get; set; }
        public string ImagePath { get; set; }
        public int CartAmount { get; set; }
        public double Points { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Enabled { get; set; }

        public UserTypeEnum UserType { get; set; }
    }
}
