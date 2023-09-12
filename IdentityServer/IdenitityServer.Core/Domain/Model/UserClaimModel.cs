namespace IdenitityServer.Core.Domain.Model
{
    public class UserClaimModel
    {
        public string UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
