using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Data.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocationArea> Areas { get; set; }
    }
}
