namespace MenuManagement_IdentityServer.Data.Models
{
    public class UserAddress
    {
        public long Id { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
    }
}
