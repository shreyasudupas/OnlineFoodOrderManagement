using MediatR;
using OrderManagement.Microservice.Core.Common.Interfaces.CartInformation;

namespace OrderManagement.Microservice.Core.Querries.CartInformationQuery
{
    public class GetIfMenuItemBelongsToSameVendorQuery : IRequest<bool>
    {
        public string UserId { get; set; }
        public string VendorId { get; set; }
    }

    public class GetIfMenuItemBelongsToSameVendorQueryHandler : IRequestHandler<GetIfMenuItemBelongsToSameVendorQuery, bool>
    {
        private readonly ICartInformationRepository _cartInformationRepository;

        public GetIfMenuItemBelongsToSameVendorQueryHandler(ICartInformationRepository cartInformationRepository)
        {
            _cartInformationRepository = cartInformationRepository;
        }

        public async Task<bool> Handle(GetIfMenuItemBelongsToSameVendorQuery request, CancellationToken cancellationToken)
        {
            return await _cartInformationRepository.CheckIfMenuItemsBelongToSameVendor(request.UserId, request.VendorId);
        }
    }
}
