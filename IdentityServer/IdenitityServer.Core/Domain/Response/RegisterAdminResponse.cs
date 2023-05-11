using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdenitityServer.Core.Domain.Response
{
    public class RegisterAdminResponse
    {
        public RegisterAdminResponse()
        {
            Errors = new List<string>();
        }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; }

        public string ReturnUrl { get; set; }

        public List<string> Errors { get; set; }
    }
}
