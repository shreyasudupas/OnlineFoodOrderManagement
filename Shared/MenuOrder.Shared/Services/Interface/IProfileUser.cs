namespace MenuOrder.Shared.Services.Interfaces
{
    public interface IProfileUser
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        (string, string) GetUserDetails();
        void SetUserDetails(string userId,string username);
    }
}
