using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class EditRolePost
    {
        public EditRolePost()
        {
            ErrorDescription = new List<string>();
        }
        public CrudEnumStatus status { get; set; }
        public List<string> ErrorDescription { get; set; }
    }
}
