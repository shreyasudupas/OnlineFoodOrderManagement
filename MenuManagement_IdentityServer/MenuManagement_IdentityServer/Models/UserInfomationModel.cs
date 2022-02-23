using MenuManagement_IdentityServer.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class UserInfomationModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserAddress> Address { get; set; }
        //public string City { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public IFormFile Photo { get; set; }
        public string ImagePath { get; set; }
    }
}
