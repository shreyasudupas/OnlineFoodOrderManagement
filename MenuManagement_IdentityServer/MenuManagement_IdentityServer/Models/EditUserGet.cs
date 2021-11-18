using MenuManagement_IdentityServer.Data.Models;

namespace MenuManagement_IdentityServer.Models
{
    public class EditUserGet
    {
        public ApplicationUser Users { get; set; }
        public string ErrorDescription { get; set; }
    }
}
