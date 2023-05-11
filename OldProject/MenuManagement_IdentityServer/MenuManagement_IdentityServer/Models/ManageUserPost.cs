namespace MenuManagement_IdentityServer.Models
{
    public class ManageUserPost
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
