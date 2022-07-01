using System.Collections.Generic;

namespace IdenitityServer.Core.Domain.DBModel
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocationArea> Areas { get; set; }
    }
}
