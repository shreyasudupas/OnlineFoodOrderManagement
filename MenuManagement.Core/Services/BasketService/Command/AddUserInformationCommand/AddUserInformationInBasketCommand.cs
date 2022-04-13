using MediatR;
using MenuManagement.Core.Common.Interfaces;
using MenuManagement.Core.Common.Models.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.BasketService.Command.AddUserInformationCommand
{
    public class AddUserInformationInBasketCommand : UserInformationModel , IRequest<bool>
    {
    }

    public class AddUserInformationInBasketCommandHandler : IRequestHandler<AddUserInformationInBasketCommand, bool>
    {
        private readonly IRedisCacheBasketService _redisCacheBasketService;
        private readonly ILogger<AddUserInformationInBasketCommandHandler> _logger;

        public AddUserInformationInBasketCommandHandler(IRedisCacheBasketService redisCacheBasketService,
            ILogger<AddUserInformationInBasketCommandHandler> logger)
        {
            _redisCacheBasketService = redisCacheBasketService;
            _logger = logger;
        }

        public async Task<bool> Handle(AddUserInformationInBasketCommand request, CancellationToken cancellationToken)
        {
            var isSucces = await _redisCacheBasketService.ManageUserInformationInBasket(request);

            _logger.LogInformation("AddUserInformationInBasketCommandHandler has added the userinformation: " + JsonConvert.SerializeObject(request) + " status: " + isSucces);

            return isSucces;
        }
    }
}
