using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Data.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<City> Cities { get; set; }
    }
}
