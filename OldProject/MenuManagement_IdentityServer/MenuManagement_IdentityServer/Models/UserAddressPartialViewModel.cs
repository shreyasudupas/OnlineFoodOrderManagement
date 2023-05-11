using MenuManagement_IdentityServer.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Models
{
    public class UserAddressPartialViewModel : BaseStatusModel
    {
        [Required]
        public string UserId { get; set; }

        public long UserAddressId { get; set; }

        [Required]
        public string FullAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }
        [Required]
        public string Area { get; set; }

        public bool IsActive { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> LocationArea { get; set; }
    }
}
