using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MicroService.Data
{
    public class Log:IEntityBase
    {
        public long Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string ErrorMessage { get; set; }
        
    }
}
