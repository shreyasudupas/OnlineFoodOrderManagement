namespace MenuManagement_IdentityServer.Models
{
    public class AddPostLogoutRedirectUriViewModel : BaseStatusModel
    {
        public string ClientId { get; set; }
        public string PostRedirectUri { get; set; }
    }
}
