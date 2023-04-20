namespace IdenitityServer.Core.Domain.DBModel
{
    public class UserAddress
    {
        public long Id { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
        public string? ApplicationUserId { get; set; }
        //These are used only by UserType is Vendor
        public string VendorId { get; set; }

        //These are used only by UserType is Vendor
        public bool Editable { get; set; }
    }
}
