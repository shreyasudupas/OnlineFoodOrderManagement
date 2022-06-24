namespace IdenitityServer.Core.Domain.Response
{
    public class LoginResponse
    {
        public bool RedirectRequired { get; set; }
        public string Error { get; set; }
    }
}
