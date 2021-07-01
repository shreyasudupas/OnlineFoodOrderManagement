using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Data
{
    public class State:IEntityBase
    {
        public long Id { get; set; }
        public string StateNames { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
