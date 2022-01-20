namespace MenuManagement_IdentityServer.Controllers.Administration
{
    public class AddUserRoleModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public bool IsSelected { get; set; }
    }
}
