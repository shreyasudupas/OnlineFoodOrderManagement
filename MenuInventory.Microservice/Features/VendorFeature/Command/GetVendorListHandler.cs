using MediatR;
using MenuInventory.Microservice.Data.Context;
using MenuInventory.Microservice.Data.MenuRepository;
using MenuInventory.Microservice.Features.VendorFeature.Querries;
using MenuInventory.Microservice.Models.Vendor;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Features.VendorFeature.Command
{
    public class GetVendorListHandler : IRequestHandler<GetAllVendorRequest, List<VendorListResponse>>
    {
        private readonly MenuInventoryContext _dbContext;
        private readonly MenuRepository _menuRepository;

        public GetVendorListHandler(MenuInventoryContext dbContext, MenuRepository menuRepository)
        {
            _dbContext = dbContext;
            _menuRepository = menuRepository;
        }

        public async Task<List<VendorListResponse>> Handle(GetAllVendorRequest request, CancellationToken cancellationToken)
        {
            //List<VendorList> Vendorlist = new List<VendorList>();
            //Vendorlist = await _dbContext.VendorLists.Where(x => x.Id > 0).ToListAsync();

            //return Vendorlist;
            List<VendorListResponse> vendorListResponses = new List<VendorListResponse>();
            var getitems = await _menuRepository.GetAllItems();
            foreach (var items in getitems)
            {
                vendorListResponses.Add(new VendorListResponse
                {
                    Id = items.Id,
                    Description = items.Description,
                    Location = items.Location,
                    Rating = items.Rating,
                    VendorName = items.VendorName
                });
            }
            return vendorListResponses;
        }
    }
}
