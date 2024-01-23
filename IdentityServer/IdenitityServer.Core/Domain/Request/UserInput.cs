using IdenitityServer.Core.Domain.Enums;

namespace IdenitityServer.Core.Domain.Request
{
    public class UserInput
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int CartAmount { get; set; }
        public double Points { get; set; }
        //public bool IsAdmin { get; set; }
        public UserTypeEnum UserType { get; set; }
        public bool Enabled { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }
        public string FullName { get; set; }
    }
}
