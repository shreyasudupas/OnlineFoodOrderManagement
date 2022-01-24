using MenuManagement_IdentityServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuManagement_IdentityServer.Models
{
    public class ClaimsViewModel
    {
        public ClaimsViewModel()
        {
            Claims = new List<ClaimDropDown>();
        }
        public List<ClaimDropDown> Claims { get; set; }
    }
}
