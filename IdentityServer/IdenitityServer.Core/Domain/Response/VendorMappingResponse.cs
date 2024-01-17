using IdenitityServer.Core.Domain.Enums;

namespace IdenitityServer.Core.Domain.Response
{
    public class VendorMappingResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string EmailId { get; set; }
        public string VendorId { get; set; }
        public bool Enabled { get; set; }
        public VendorUserTypeEnum UserType { get; set; }
    }
}
