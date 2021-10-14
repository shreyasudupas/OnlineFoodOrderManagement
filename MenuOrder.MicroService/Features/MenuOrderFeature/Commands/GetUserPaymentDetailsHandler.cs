using AutoMapper;
using Common.Utility.Models.CartInformationModels;
using MediatR;
using MenuOrder.MicroService.Features.MenuOrderFeature.Querries;
using MenuOrder.MicroService.Features.MenuOrderFeature.Response;
using MenuOrder.MicroService.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MenuOrder.MicroService.Features.MenuOrderFeature.Commands
{
    public class GetUserPaymentDetailsHandler : IRequestHandler<GetUserPaymentDetails, UserPaymentDetailsReponse>
    {
        private readonly HttpClientCrudService _httpClientCrud;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetUserPaymentDetailsHandler(HttpClientCrudService httpClientCrud, IConfiguration configuration, IMapper mapper)
        {
            _httpClientCrud = httpClientCrud;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserPaymentDetailsReponse> Handle(GetUserPaymentDetails request, CancellationToken cancellationToken)
        {
            UserPaymentDetailsReponse PaymentInfo = new UserPaymentDetailsReponse();

            var UserBasketInfo = await _httpClientCrud.GetItemsAsync(_configuration["BaseApiUrls:BasketBaseUrl"], 
                _configuration["BaseApiUrls:BasketActionUrl"] + "GetUserBasketInfoFromCache",
                request.Token ,
                request.UserInfo);

            if(UserBasketInfo != null)
            {
                 var CartResponse= JsonConvert.DeserializeObject<UserCartInformation>(UserBasketInfo);
                    
                //Calcualte total Amount
                 PaymentInfo = _mapper.Map<UserPaymentDetailsReponse>(CartResponse);
            }

            return PaymentInfo;
        }
    }
}
