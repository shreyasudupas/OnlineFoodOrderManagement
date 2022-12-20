namespace IdenitityServer.Core.Domain.Request
{
    public class UserInput
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int CartAmount { get; set; }
        public double Points { get; set; }
        public bool IsAdmin { get; set; }
    }
}
