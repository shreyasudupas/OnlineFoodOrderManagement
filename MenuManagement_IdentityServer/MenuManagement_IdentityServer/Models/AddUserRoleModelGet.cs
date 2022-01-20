using MenuManagement_IdentityServer.Controllers.Administration;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class AddUserRoleModelGet
    {
        public AddUserRoleModelGet()
        {
            ListUsersRole = new List<AddUserRoleModel>();
        }
        public string ErrorDescription { get; set; }
        public List<AddUserRoleModel> ListUsersRole { get; set; }
        public CrudEnumStatus Status { get; set; }
    }
}
