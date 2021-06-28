
using MicroService.Shared.BuisnessLayer.IBuisnessLayer;

namespace MicroService.Shared.BuisnessLayer
{
    /// <summary>
    /// This is used to send the Email Id across the diffrent layers in Scopped Lifetime from Middleware to Authorization to Controller the value Email Id can be read
    /// </summary>
    /// <retruns>The EmailID</retruns>
    public class ProfileUser : IProfileUser
    {
        //public string EmailId { get; set; }
        private string EmailId;
        private string PictureLocation;
        private string NickName;
        private int RoleId;

        public (string, string, string, int) GetUserDetails()
        {
            return (EmailId, PictureLocation, NickName, RoleId);
        }

        public void SetUserDetails(string email, string PicLocation, string Nickname, int RoleId)
        {
            EmailId = email;
            PictureLocation = PicLocation;
            this.NickName = Nickname;
            this.RoleId = RoleId;
        }
    }
}
