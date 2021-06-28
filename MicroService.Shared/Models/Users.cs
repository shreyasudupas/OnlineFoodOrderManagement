namespace MicroService.Shared.Models
{
    public class Users
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string PictureLocation { get; set; }
        public long Points { get; set; }
        public double CartAmount { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
    }
}
