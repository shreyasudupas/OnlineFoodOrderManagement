using IdenitityServer.Core.Domain.Response;
using System.ComponentModel.DataAnnotations;

namespace IdenitityServer.Core.Domain.Model
{
    public class VendorRegisterModel : RegisterResponse
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
