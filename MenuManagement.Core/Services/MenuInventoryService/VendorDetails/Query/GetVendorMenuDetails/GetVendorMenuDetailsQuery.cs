using AutoMapper;
using MediatR;
using MenuManagment.Domain.Mongo.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.MenuInventoryService.VendorDetails.Query.GetVendorMenuDetails
{
    public class GetVendorMenuDetailsQuery : IRequest<VendorMenuDetailDto>
    {
        public string Location { get; set; }
        public string VendorId { get; set; }
    }

    public class GetVendorMenuDetailsQueryHandler : IRequestHandler<GetVendorMenuDetailsQuery, VendorMenuDetailDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetVendorMenuDetailsQueryHandler(IMenuRepository menuRepository,
            IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<VendorMenuDetailDto> Handle(GetVendorMenuDetailsQuery request, CancellationToken cancellationToken)
        {
            VendorMenuDetailDto result = new VendorMenuDetailDto();
            var getMenuDetails = await _menuRepository.ListVendorMenuDetails(request.VendorId,request.Location);

            if(getMenuDetails!= null)
            {
                var menuDetailModelToDto = _mapper.Map<List<MenuColumnDetailDto>>(getMenuDetails.ColumnDetail);
                result.MenuColumnDetail = menuDetailModelToDto;
                result.Data = getMenuDetails.Data;
            }

            return result;
        }
    }
}
