using MediatR;
using MenuInventory.Microservice.Data.Context;
using MenuInventory.Microservice.Data.MenuRepository;
using MenuInventory.Microservice.Features.MenuFeature.Querries;
using MenuInventory.Microservice.Models;
using MenuInventory.Microservice.Models.Menu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Features.MenuFeature.Command
{
    public class GetMenuBasedOnVendorHandler : IRequestHandler<VendorIdRequest, MenuListReposnse>
    {
        //private readonly MenuInventoryContext _dbContext;
        private readonly MenuRepository menuRepository;
        public GetMenuBasedOnVendorHandler(MenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        public async Task<MenuListReposnse> Handle(VendorIdRequest request, CancellationToken cancellationToken)
        {
            MenuListReposnse ListMenu = new MenuListReposnse();
            
            var vendorItem = await menuRepository.GetById(request.VendorId);

            if(vendorItem.VendorDetails != null && vendorItem.VendorDetails.ColumnDetails.Count >0 && vendorItem.VendorDetails.Data != null)
            {
                var VendorColumnData = vendorItem.VendorDetails.ColumnDetails;

                //prepare the column 
                List<MenuColumnData> ColumnData = new List<MenuColumnData>();
                foreach (var column in VendorColumnData)
                {
                    ColumnData.Add(new MenuColumnData { Field = column.ColumnName, Header = column.DisplayName, Display = column.Display });
                }

                //prepare the data
                ListMenu.Data = vendorItem.VendorDetails.Data;
                ListMenu.MenuColumnData = ColumnData;
            }
            

            return ListMenu;
        }
    }
}
