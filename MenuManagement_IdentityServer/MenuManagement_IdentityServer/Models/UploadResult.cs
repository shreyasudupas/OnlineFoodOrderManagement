using Microsoft.AspNetCore.Http;

namespace MenuManagement_IdentityServer.Models
{
    public class UploadResult
    {
        public IFormFile UploadFile { get; set; }
        public UserInfomationModel User { get; set; }
    }
}
