using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.Vendor.Commands.UpdateVendorRegistration
{
    public class UpdateVendorRegistrationCommand : IRequest<bool>
    {
        public string VendorId { get; set; }
        public string VendorStatus { get; set; }
    }

    public class UpdateVendorRegistrationCommandHandler : IRequestHandler<UpdateVendorRegistrationCommand, bool>
    {
        private readonly IVendorRepository _vendorRepository;

        public UpdateVendorRegistrationCommandHandler(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<bool> Handle(UpdateVendorRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await _vendorRepository.UpdateVendorStatus(request.VendorId, request.VendorStatus);
        }
    }
}
