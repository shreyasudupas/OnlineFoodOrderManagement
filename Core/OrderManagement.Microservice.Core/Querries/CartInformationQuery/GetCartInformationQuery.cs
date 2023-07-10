using AutoMapper;
using MediatR;
using MenuManagment.Mongo.Domain.Dtos.OrderManagement;
using OrderManagement.Microservice.Core.Common.Interfaces.CartInformation;

namespace OrderManagement.Microservice.Core.Querries.CartInformationQuery
{
    public class GetCartInformationQuery : IRequest<CartInformationDto>
    {
        public string UserId { get; set; }
    }

    public class GetCartInformationQueryHandler : IRequestHandler<GetCartInformationQuery, CartInformationDto>
    {
        private readonly ICartInformationRepository _cartInformationRepository;
        private readonly IMapper _mapper;

        public GetCartInformationQueryHandler(ICartInformationRepository cartInformationRepository,
            IMapper mapper)
        {
            _cartInformationRepository = cartInformationRepository;
            _mapper = mapper;
        }

        public async Task<CartInformationDto> Handle(GetCartInformationQuery request, CancellationToken cancellationToken)
        {
            var result = await _cartInformationRepository.GetActiveUserCartInformation(request.UserId);

            var mapToDto = _mapper.Map<CartInformationDto>(result);

            return mapToDto;
        }
    }
}
