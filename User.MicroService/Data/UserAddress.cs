namespace Identity.MicroService.Data
{
    public class UserAddress : IEntityBase
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Address { get; set; }
        public long? CityId { get; set; }
        public User UserEntityFK { get; set; }
        public City CityEntityFK { get; set; }
    }
}
