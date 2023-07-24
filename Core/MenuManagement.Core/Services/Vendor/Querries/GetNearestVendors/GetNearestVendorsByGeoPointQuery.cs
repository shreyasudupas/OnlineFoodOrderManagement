using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.Vendor.Querries.GetNearestVendors
{
    public class GetNearestVendorsByGeoPointQuery : IRequest<List<VendorDto>>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceInKm { get; set; }
    }

    public class GetNearestVendorsByGeoPointQueryHandler : IRequestHandler<GetNearestVendorsByGeoPointQuery, List<VendorDto>>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public GetNearestVendorsByGeoPointQueryHandler(IVendorRepository vendorRepository,
            IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<List<VendorDto>> Handle(GetNearestVendorsByGeoPointQuery request, CancellationToken cancellationToken)
        {
            var nearestVendors = await _vendorRepository.GetNearestDistanceOfVendorsByRadiusInKM(request.Latitude,
                request.Longitude,request.DistanceInKm);

            var mapToDtos = _mapper.Map<List<VendorDto>>(nearestVendors);

            return mapToDtos;
        }
    }
}
