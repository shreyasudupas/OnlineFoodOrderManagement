using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace IdenitityServer.Core.Domain.Request
{
    public class UserProfilePicture
    {
        [FromForm(Name = "image")]
        public IFormFile Image { get; set; }

        [FromForm(Name = "userId")]
        public string UserId { get; set; }
    }
}
