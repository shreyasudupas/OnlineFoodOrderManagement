using MediatR;
using MenuInventory.Microservice.Data.Context;
using MenuInventory.Microservice.Features.MenuFeature.Querries;
using MenuInventory.Microservice.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Features.MenuFeature.Command
{
    public class GetMenuBasedOnVendorHandler : IRequestHandler<VendorIdRequest, MenuDisplayList>
    {
        private readonly MenuInventoryContext _dbContext;
        public GetMenuBasedOnVendorHandler(MenuInventoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MenuDisplayList> Handle(VendorIdRequest request, CancellationToken cancellationToken)
        {
            MenuDisplayList ListMenu = new MenuDisplayList();

            var MenuList = await(from Menu in _dbContext.Menus
                                 join MenuTypeName in _dbContext.MenuTypes on Menu.MenuTypeId equals MenuTypeName.Id
                                 where (Menu.VendorId == request.VendorId)
                                 orderby Menu.MenuTypeId
                                 select new MenuList
                                 {
                                     MenuId = Menu.Id,
                                     MenuItem = Menu.MenuItem,
                                     Price = Menu.Price,
                                     VendorId = Menu.VendorId,
                                     MenuType = MenuTypeName.MenuTypeName,
                                     ImagePath = Menu.ImagePath,
                                     OfferPrice = Menu.OfferPrice,
                                     CreatedDate = Menu.CreatedDate
                                 }).ToListAsync();

            var GetImageLinkOfMenuType = await(from type in _dbContext.MenuTypes
                                               where (type.Id > 0)
                                               select new MenuItemDetail
                                               {
                                                   MenuTypeId = type.Id,
                                                   MenuTypeName = type.MenuTypeName,
                                                   ImagePath = type.ImagePath
                                               }).ToListAsync();

            ListMenu.MenuItemDetails = GetImageLinkOfMenuType;
            ListMenu.MenuItemList = MenuList;

            return ListMenu;
        }
    }
}
