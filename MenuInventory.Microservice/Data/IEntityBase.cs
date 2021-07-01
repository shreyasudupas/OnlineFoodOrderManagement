using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Data
{
    public interface IEntityBase
    {
        public long Id { get; set; }
    }
}
