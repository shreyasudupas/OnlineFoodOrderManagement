using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class ManagerUserRole
    {
        public ManagerUserRole()
        {
            Role = new List<UserRoleSelection>();
            ErrorDescription = new List<string>();
        }
        public string UserId { get; set; }
        public List<UserRoleSelection> Role { get; set; }
        public CrudEnumStatus status { get; set; }

        public List<string> ErrorDescription { get; set; }
    }

    public class UserRoleSelection
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
