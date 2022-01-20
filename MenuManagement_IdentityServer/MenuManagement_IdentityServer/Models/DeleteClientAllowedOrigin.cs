namespace MenuManagement_IdentityServer.Models
{
    public class DeleteClientAllowedOrigin
    {
        public string ClientId { get; set; }
        public int AllowedClientOriginId { get; set; }
    }
}
