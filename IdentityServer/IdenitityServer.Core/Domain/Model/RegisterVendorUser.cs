using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdenitityServer.Core.Domain.Model
{
    public class RegisterVendorUser
    {
        [Required]
        public string VendorId { get; set; }

        [Required]
        public string Vendorname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public List<string> Errors { get; set; }
    }
}
