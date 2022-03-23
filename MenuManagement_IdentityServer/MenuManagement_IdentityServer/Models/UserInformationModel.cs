using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class UserInformationModel : BaseStatusModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserAdressModel> Address { get; set; }
        
        public string ImagePath { get; set; }
        public int CartAmount { get; set; }
        public double Points { get; set; }
    }

    public class UserAdressModel
    {
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
    }
}
