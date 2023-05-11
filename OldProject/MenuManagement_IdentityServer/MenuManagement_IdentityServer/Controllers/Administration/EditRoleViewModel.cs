using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuManagement_IdentityServer.Controllers.Administration
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            //collecion are not initilized so add them like this
            UserNames = new List<string>();
        }
        public string Id { get; set; }
        [Required(ErrorMessage ="Role Name is required")]
        public string RoleName { get; set; }
        public List<string> UserNames { get; set; }
    }
}
