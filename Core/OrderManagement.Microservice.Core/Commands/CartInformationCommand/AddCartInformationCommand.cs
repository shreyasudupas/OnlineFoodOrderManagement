using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using MenuManagment.Mongo.Domain.Entities;
using OrderManagement.Microservice.Core.Common.Interfaces.CartInformation;

namespace OrderManagement.Microservice.Core.Commands.CartInformationCommand
{
    public class AddCartInformationCommand : CartInformationDto, IRequest<CartInformationDto>
    {
    }

    public class AddCartInformationCommandHandler : IRequestHandler<AddCartInformationCommand, CartInformationDto>
    {
        private readonly ICartInformationRepository _cartInformationRepository;
        private readonly IMapper _mapper;

        public AddCartInformationCommandHandler(ICartInformationRepository cartInformationRepository,
            IMapper mapper)
        {
            _cartInformationRepository = cartInformationRepository;
            _mapper = mapper;
        }

        public async Task<CartInformationDto> Handle(AddCartInformationCommand request, 
            CancellationToken cancellationToken)
        {
            if(!string.IsNullOrEmpty(request.UserId))
            {
                var cartModel = _mapper.Map<CartInformation>(request);

                var result = await _cartInformationRepository.AddUserCartInformation(cartModel);

                var mapResultToDto = _mapper.Map<CartInformationDto>(result);

                return mapResultToDto;
            }
            else
            {
                throw new Exception("User Id is empty");
            }
        }
    }
}
