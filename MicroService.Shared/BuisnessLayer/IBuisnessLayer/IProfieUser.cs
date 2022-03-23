namespace Common.Utility.BuisnessLayer.IBuisnessLayer
{
    public interface IProfileUser
    {
        //public string EmailId { get; set; }

        (string, string, string, int) GetUserDetails();
        void SetUserDetails(string email, string PicLocation, string Nickname, int RoleId);
    }
}
