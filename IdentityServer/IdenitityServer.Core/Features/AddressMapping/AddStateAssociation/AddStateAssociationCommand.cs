using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Features.AddressMapping.AddStateAssociation
{
    public class AddStateAssociationCommand : IRequest<RegisteredLocationReponse>
    {
        public string State { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
    }

    public class AddStateAssociationCommandHandler : IRequestHandler<AddStateAssociationCommand, RegisteredLocationReponse>
    {
        private readonly IAddressService _addressService;

        public AddStateAssociationCommandHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<RegisteredLocationReponse> Handle(AddStateAssociationCommand request, CancellationToken cancellationToken)
        {
            var response = new RegisteredLocationReponse();

            var stateInfo = new State();
            var cityInfo = new City();
            var areaInfo = new LocationArea();

            stateInfo.Name = request.State;
            cityInfo.Name = request.City;
            areaInfo.AreaName = request.Area;

            var result = await _addressService.AddAllStateAssociation(stateInfo, cityInfo, areaInfo);
            response.State = result;

            return response;
        }
    }
}
