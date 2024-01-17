using IdenitityServer.Core.Domain.Enums;

namespace IdenitityServer.Core.Domain.DBModel
{
    public class VendorUserIdMapping
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string? VendorId { get; set; }
        public string EmailId { get; set; }
        public VendorUserTypeEnum UserType { get; set; }
    }
}
