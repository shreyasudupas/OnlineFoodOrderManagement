using MenuManagement_IdentityServer.Data.Models;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class EditUser  : BaseStatusModel
    {
        public EditUser()
        {
            ErrorDescription = new List<string>();
        }
        public ApplicationUser User { get; set; }
    }
}
