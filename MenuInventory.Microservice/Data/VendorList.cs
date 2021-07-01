using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Data
{
    public class VendorList : IEntityBase
    {
        public long Id { get; set; }
        public string VendorName { get; set; }
        public string VendorDescription { get; set; }
        public int? VendorRating { get; set; }
        public string VendorImgLink { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
