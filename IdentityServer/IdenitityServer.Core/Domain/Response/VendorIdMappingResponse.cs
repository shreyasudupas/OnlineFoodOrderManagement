namespace IdenitityServer.Core.Domain.Response
{
    public class VendorIdMappingResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? VendorId { get; set; }
        public bool Enabled { get; set; }
    }
}
