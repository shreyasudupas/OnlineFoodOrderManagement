using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Cities = new List<SelectListItem>();
            States = new List<SelectListItem>();
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
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string ReturnUrl { get; set; }

        public List<SelectListItem> Cities { get; set; }

        public List<SelectListItem> States { get; set; }
        //public List<string> ErrorList { get; set; }
    }
}
