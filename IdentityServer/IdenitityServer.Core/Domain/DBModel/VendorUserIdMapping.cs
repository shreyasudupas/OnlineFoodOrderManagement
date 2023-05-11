namespace IdenitityServer.Core.Domain.DBModel
{
    public class VendorUserIdMapping
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? VendorId { get; set; }
        public bool Enabled { get; set; }
    }
}
