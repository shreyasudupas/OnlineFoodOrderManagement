using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdenitityServer.Core.Domain.Response
{
    public class RegisterResponse
    {
        public RegisterResponse()
        {
            Cities = new List<SelectListItem>();
            States = new List<SelectListItem>();
            Areas = new List<SelectListItem>();
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
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }
        [Required]
        public string Area { get; set; }

        public string ReturnUrl { get; set; }

        public List<SelectListItem> Cities { get; set; }

        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Areas { get; set; }
        public List<string> Errors { get; set; }
    }
}
