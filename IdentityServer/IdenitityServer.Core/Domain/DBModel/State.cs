using System.Collections.Generic;

namespace IdenitityServer.Core.Domain.DBModel
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<City> Cities { get; set; }
    }
}
