using System;

namespace Identity.MicroService.Data
{
    public class City: IEntityBase
    {
        public long Id { get; set; }
        public string CityNames { get; set; }
        public long StateId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public State StateEntityFK { get; set; }
        //public ICollection<User> Users { get; set; }
    }
}
