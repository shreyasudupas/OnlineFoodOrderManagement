using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdenitityServer.Core.Domain.Response
{
    public class RegisterResponse
    {
        public RegisterResponse()
        {
            Errors = new List<string>();
        }
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

        public string ReturnUrl { get; set; }
        
        public List<string> Errors { get; set; }
    }
}
