using AutoMapper;
using Common.Mongo.Database.Data.Context;
using MediatR;
using MenuInventory.Microservice.Features.VendorCartConfigurationFeature.Querries;
using MenuInventory.Microservice.Models.Vendor;
using MenuInventory.Microservice.Models.VendorCartConfiguration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Features.VendorCartConfigurationFeature.Command
{
    public class GetVendorConfigurationHandler : IRequestHandler<GetVendorCartConfig, VendorCartConfigurationResponse>
    {
        private readonly VendorCartRepository _vendorCartRepository;
        private readonly IMapper _mapper;

        public GetVendorConfigurationHandler(VendorCartRepository vendorCartRepository, IMapper mapper)
        {
            _vendorCartRepository = vendorCartRepository;
            _mapper = mapper;
        }

        public async Task<VendorCartConfigurationResponse> Handle(GetVendorCartConfig request, CancellationToken cancellationToken)
        {
            var VendorReponse = new VendorCartConfigurationResponse();
            //var GetVendorConfigData = await _vendorCartRepository.GetById(request.vendorId);
            var GetVendorConfigData = await _vendorCartRepository.GetVendorConfigurationByVendorId(request.vendorId);

            if (GetVendorConfigData != null)
            {
                var VendorResponseMap = _mapper.Map<VendorListResponse>(GetVendorConfigData.VendorDetails);
                var ColumnMap = _mapper.Map<List<ColumnDetails>>(GetVendorConfigData.ColumnDetails);
                VendorReponse.ColumnDetails = ColumnMap;
                VendorReponse.VendorDetails = VendorResponseMap;
            }
            return VendorReponse;
        }
    }
}
