namespace IdenitityServer.Core.Domain.Response
{
    public class VendorUserMappingEnableResponse
    {
        public string VendorId { get; set; }
        public bool IsVendorPresent { get; set; }

        public bool IsEnabled { get; set; }
    }
}
