using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Data
{
    public class UserRole:IEntityBase
    {
        public long Id { get; set; }
        public string Rolename { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
