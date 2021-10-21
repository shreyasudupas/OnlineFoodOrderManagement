using System.Collections.Generic;

namespace Common.Utility.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public List<UserAddress> Address { get; set; }
        public string PictureLocation { get; set; }
        public long Points { get; set; }
        public double CartAmount { get; set; }

    }

    public class UserAddress
    {
        public long UserAddressId { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
    }
}
