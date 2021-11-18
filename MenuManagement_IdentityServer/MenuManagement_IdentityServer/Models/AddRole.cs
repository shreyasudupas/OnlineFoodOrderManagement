using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class AddRole
    {
        public AddRole()
        {
            ErrorDescription = new List<string>();
        }
        public string RoleName { get; set; }
        public List<string> ErrorDescription { get; set; }
    }
}
