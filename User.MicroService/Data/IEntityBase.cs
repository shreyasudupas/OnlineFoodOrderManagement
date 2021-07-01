using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Data
{
    public interface IEntityBase
    {
        public long Id { get; set; }
    }
}
