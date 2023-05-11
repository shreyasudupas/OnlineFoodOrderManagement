using Microsoft.AspNetCore.Http;

namespace IdenitityServer.Core.Domain.Request
{
    public class UserProfilePicture
    {
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
    }
}
