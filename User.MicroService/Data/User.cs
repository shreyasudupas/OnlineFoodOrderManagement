using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Data
{
    public class User:IEntityBase
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        //foriegn key
        public long RoleId { get; set; }
        public string FullName { get; set; }

        public string? Address { get; set; }
        public long? CityId { get; set; }
        public long? StateId { get; set; }
        public string PictureLocation { get; set; }
        public long Points { get; set; }
        public double CartAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }



        public UserRole Role { get; set; }
        public City City { get; set; }
        public State State { get; set; }
    }
}
