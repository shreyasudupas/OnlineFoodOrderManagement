using MenuInventory.Microservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.IBuisnessLayer
{
    public interface IMenuBL
    {
        Task<MenuDisplayList> GetMenuListForVednorId(int VendorId);
    }
}
