namespace IdenitityServer.Core.Domain.Request
{
    public class ImageUploadRequest
    {
        public string UserId { get; set; }

        public string ImageUrl { get; set; }
        public string Type { get; set; }
    }
}
