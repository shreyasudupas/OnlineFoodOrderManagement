using MenuDatabase.Data.Database;
using MenuInventory.Microservice.IBuisnessLayer;
using MenuInventory.Microservice.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.BuisnessLayer
{
    public class MenuBL : IMenuBL
    {
        private readonly MenuOrderManagementContext _dbContext;

        public MenuBL(MenuOrderManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MenuDisplayList> GetMenuListForVednorId(int VendorId)
        {
            MenuDisplayList ListMenu = new MenuDisplayList();

            //ListMenu = dbContext.TblMenus.Where(x => x.VendorId == VendorId).OrderBy(x=>x.MenuTypeId).ToList();
            var MenuList = await (from Menu in _dbContext.TblMenus
                                  join MenuTypeName in _dbContext.TblMenuTypes on Menu.MenuTypeId equals MenuTypeName.MenuTypeId
                                  where (Menu.VendorId == VendorId)
                                  orderby Menu.MenuTypeId
                                  select new MenuList
                                  {
                                      MenuId = Menu.MenuId,
                                      MenuItem = Menu.MenuItem,
                                      Price = Menu.Price,
                                      VendorId = Menu.VendorId,
                                      MenuType = MenuTypeName.MenuTypeName,
                                      ImagePath = Menu.ImagePath,
                                      OfferPrice = Menu.OfferPrice,
                                      CreatedDate = Menu.CreatedDate
                                  }).ToListAsync();

            var GetImageLinkOfMenuType = await (from type in _dbContext.TblMenuTypes
                                                where (type.MenuTypeId > 0)
                                                select new MenuItemDetail
                                                {
                                                    MenuTypeId = type.MenuTypeId,
                                                    MenuTypeName = type.MenuTypeName,
                                                    ImagePath = type.ImagePath
                                                }).ToListAsync();

            ListMenu.MenuItemDetails = GetImageLinkOfMenuType;
            ListMenu.MenuItemList = MenuList;

            return ListMenu;
        }
    }
}
