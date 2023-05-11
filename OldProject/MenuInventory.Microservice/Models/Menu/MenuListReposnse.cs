using System.Collections.Generic;

namespace MenuInventory.Microservice.Models.Menu
{
    public class MenuListReposnse
    {
        public List<MenuColumnData> MenuColumnData { get; set; }
        public List<object> Data { get; set; }
    }
}
