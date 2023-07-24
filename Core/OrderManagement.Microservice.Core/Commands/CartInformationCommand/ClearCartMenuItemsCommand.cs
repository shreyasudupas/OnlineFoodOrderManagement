using MediatR;
using OrderManagement.Microservice.Core.Common.Interfaces.CartInformation;

namespace OrderManagement.Microservice.Core.Commands.CartInformationCommand
{
    public class ClearCartMenuItemsCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }

    public class ClearCartMenuItemsCommandHandler : IRequestHandler<ClearCartMenuItemsCommand, bool>
    {
        private readonly ICartInformationRepository _cartInformationRepository;

        public ClearCartMenuItemsCommandHandler(ICartInformationRepository cartInformationRepository)
        {
            _cartInformationRepository = cartInformationRepository;
        }

        public async Task<bool> Handle(ClearCartMenuItemsCommand request, CancellationToken cancellationToken)
        {
            return await _cartInformationRepository.CartActiveMenuItemsClear(request.UserId);
        }
    }
}
