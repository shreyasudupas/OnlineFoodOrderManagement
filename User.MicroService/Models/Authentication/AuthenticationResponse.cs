namespace Identity.MicroService.Models.Authentication
{
    public class AuthenticationResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string PictureLocation { get; set; }
        public string Nickname { get; set; }
        public string Token { get; set; }
    }
}
