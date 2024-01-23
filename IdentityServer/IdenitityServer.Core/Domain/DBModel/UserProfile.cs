using System;
using System.Collections.Generic;
using IdenitityServer.Core.Domain.Enums;
using IdenitityServer.Core.Domain.Model;

namespace IdenitityServer.Core.Domain.DBModel
{
    public class UserProfile
    {
        public UserProfile()
        {
            Address = new List<UserProfileAddress>();
            Claims = new List<DropdownModel>();
            Roles = new List<DropdownModel>();
        }

        public string Id { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string SecurityStamp { get; set; }

        public string PasswordHash { get; set; }

        public bool EmailConfirmed { get; set; }

        public string NormalizedEmail { get; set; }

        public string Email { get; set; }

        public string NormalizedUserName { get; set; }

        public string UserName { get; set; }


        public bool LockoutEnabled { get; set; }

        public DateTime CreatedDate { get; set; }

        public int AccessFailedCount { get; set; }
        public List<UserProfileAddress> Address { get; set; }
        //public bool IsAdmin { get; set; }
        public string ImagePath { get; set; }
        public int CartAmount { get; set; }
        public double Points { get; set; }

        public List<DropdownModel> Claims { get; set; }
        public List<DropdownModel> Roles { get; set; }
        public bool Enabled { get; set; }
        public UserTypeEnum UserType { get; set; }

        public string Fullname { get; set; }
    }
}
