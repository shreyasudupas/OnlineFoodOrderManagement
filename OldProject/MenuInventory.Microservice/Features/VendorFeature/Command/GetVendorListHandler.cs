using Common.Mongo.Database.Data.Context;
using MediatR;
using MenuInventory.Microservice.Features.VendorFeature.Querries;
using MenuInventory.Microservice.Models.Vendor;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Features.VendorFeature.Command
{
    public class GetVendorListHandler : IRequestHandler<GetAllVendorRequest, List<VendorListResponse>>
    {
        private readonly MenuRepository _menuRepository;

        public GetVendorListHandler(MenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<VendorListResponse>> Handle(GetAllVendorRequest request, CancellationToken cancellationToken)
        {
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
