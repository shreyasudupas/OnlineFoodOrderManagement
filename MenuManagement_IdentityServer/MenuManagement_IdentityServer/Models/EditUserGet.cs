using MenuManagement_IdentityServer.Data.Models;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class EditUserGet
    {
        public EditUserGet()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
        public ApplicationUser Users { get; set; }
        public IList<string> Roles { get; set; }
        public List<string> Claims { get; set; }
        public string ErrorDescription { get; set; }
    }
}
