using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Data
{
    public class MenuType:IEntityBase
    {
        public long Id { get; set; }
        public string MenuTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
