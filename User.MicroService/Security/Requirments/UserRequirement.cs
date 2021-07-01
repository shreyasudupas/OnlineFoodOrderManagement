using Microsoft.AspNetCore.Authorization;

namespace Identity.MicroService.Security.Requirments
{
    public class UserRequirement : IAuthorizationRequirement
    {
        public string User { get; set; }
        public UserRequirement(string User)
        {
            this.User = User;
        }
    }
}
