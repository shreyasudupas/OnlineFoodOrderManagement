using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Controllers.Administration
{
    public class AddRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
