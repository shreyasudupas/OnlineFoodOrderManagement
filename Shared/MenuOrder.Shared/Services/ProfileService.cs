using MenuOrder.Shared.Services.Interfaces;

namespace Inventory.Microservice.Core.Common.Services
{
    /// <summary>
    /// This is used to send the Email Id across the diffrent layers in Scopped Lifetime from Middleware to Authorization to Controller the value Email Id can be read
    /// </summary>
    /// <retruns>The EmailID</retruns>
    public class ProfileUser : IProfileUser
    {
        public string UserId { get; set; }
        public string Username { get; set; }

        public void SetUserDetails(string userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        (string, string) IProfileUser.GetUserDetails()
        {
            return (UserId, Username);
        }
    }
}
