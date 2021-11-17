using MenuManagement_IdentityServer.Data.Models;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class EditUser
    {
        public EditUser()
        {
            ErrorDescription = new List<string>();
        }
        public ApplicationUser User { get; set; }
        public List<string> ErrorDescription { get; set; }
    }
}
