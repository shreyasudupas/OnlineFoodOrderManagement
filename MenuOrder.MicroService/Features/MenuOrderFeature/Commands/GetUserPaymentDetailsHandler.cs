using AutoMapper;
using Common.Utility.Models.CartInformationModels;
using MediatR;
using MenuOrder.MicroService.Features.MenuOrderFeature.Querries;
using MenuOrder.MicroService.Features.MenuOrderFeature.Response;
using MenuOrder.MicroService.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MenuOrder.MicroService.Features.MenuOrderFeature.Commands
{
    public class GetUserPaymentDetailsHandler : IRequestHandler<GetUserPaymentDetails, UserPaymentDetailsReponse>
    {
        private readonly HttpClientCrudService _httpClientCrud;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly MenuConfigurationService _menuConfigurationService;

        public GetUserPaymentDetailsHandler(HttpClientCrudService httpClientCrud, IConfiguration configuration, IMapper mapper, MenuConfigurationService menuConfigurationService)
        {
            _httpClientCrud = httpClientCrud;
            _configuration = configuration;
            _mapper = mapper;
            _menuConfigurationService = menuConfigurationService;
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
                 if(CartResponse.Items != null)
                {
                    int TotalPrice = 0;

                    var GetPriceColumnNameOfVendor = await _menuConfigurationService.GetColumnNameBasedOnVendorId(CartResponse.VendorDetails.Id,
                        _configuration["VendorColumnConfiguration:Column-Price"]);

                    var GetTotalQuantity = CartResponse.Items.Select(x => new { 
                        Quantity = Convert.ToInt32(x[_configuration["VendorColumnConfiguration:Column-Quantity"]]) ,
                        Price = Convert.ToInt32(x[GetPriceColumnNameOfVendor])
                    }).ToList();

                    GetTotalQuantity.ForEach(element =>
                    {
                        TotalPrice += element.Quantity * element.Price;
                    });

                    PaymentInfo.TotalAmount = TotalPrice;
                    PaymentInfo.UserInfo = CartResponse.UserInfo;

                    //Also get if user has chosen a predefined choice of payment add that in Payment Info
                }

                
            }

            return PaymentInfo;
        }
    }
}
