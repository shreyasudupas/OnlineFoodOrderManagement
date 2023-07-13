using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using OrderManagement.Microservice.Core.Common.Interfaces.CartInformation;

namespace OrderManagement.Microservice.Core.Commands.CartInformationCommand
{
    public class UpdateCartInformationCommand : CartInformationDto , IRequest<CartInformationDto?>
    {
    }

    public class UpdateCartInformationCommandHandler : IRequestHandler<UpdateCartInformationCommand, CartInformationDto?>
    {
        private readonly ICartInformationRepository _cartInformationRepository;
        private readonly IMapper _mapper;

        public UpdateCartInformationCommandHandler(ICartInformationRepository cartInformationRepository,
            IMapper mapper)
        {
            _cartInformationRepository = cartInformationRepository;
            _mapper = mapper;
        }

        public async Task<CartInformationDto?> Handle(UpdateCartInformationCommand request, CancellationToken cancellationToken)
        {
            var mapDtoToModel = _mapper.Map<CartInformation>(request);
            var result = await _cartInformationRepository.UpdateCartInformation(mapDtoToModel);

            var mapToDto = _mapper.Map<CartInformationDto>(result);

            return mapToDto;
        }
    }
}
