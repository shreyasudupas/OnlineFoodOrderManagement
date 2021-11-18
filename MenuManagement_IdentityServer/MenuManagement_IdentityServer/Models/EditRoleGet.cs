using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class EditRoleGet
    {
        public EditRoleGet()
        {
            Username = new List<string>();
        }
        public CrudEnumStatus status { get; set; }
        public List<string> Username { get; set; }
        public string Id { get; set; }
        public string RoleName { get; set; }
    }
}
