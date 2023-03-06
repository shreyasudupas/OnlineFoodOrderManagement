using IdenitityServer.Core.Domain.Response;
using System.ComponentModel.DataAnnotations;

namespace IdenitityServer.Core.Domain.Model
{
    public class VendorRegister : RegisterResponse
    {
        [Required]
        public string VendorName { get; set; }

        public string VendorDescription { get; set; }

    }
}
