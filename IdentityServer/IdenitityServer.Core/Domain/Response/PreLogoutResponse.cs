namespace IdenitityServer.Core.Domain.Response
{
    public class PreLogoutResponse
    {
        public string LogoutId { get; set; }
        public bool ShowLogoutPrompt { get; set; }
    }
}
