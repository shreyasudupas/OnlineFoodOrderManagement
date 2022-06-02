using MediatR;
using MenuManagement.Core.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MenuManagement.Core.Services.BasketService.Command
{
    public class DeleteUserCartItemCommand : IRequest<bool>
    {
        public string Username { get; set; }
    }

    public class DeleteUserCartItemCommandHandler : IRequestHandler<DeleteUserCartItemCommand, bool>
    {
        private readonly IRedisCacheBasketService _redisCacheBasketService;
        private readonly ILogger<DeleteUserCartItemCommandHandler> _logger;

        public DeleteUserCartItemCommandHandler(IRedisCacheBasketService redisCacheBasketService
            ,ILogger<DeleteUserCartItemCommandHandler> logger)
        {
            _redisCacheBasketService = redisCacheBasketService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteUserCartItemCommand request, CancellationToken cancellationToken)
        {
            var userCartInfo = await _redisCacheBasketService.GetBasketItems(request.Username);

            if(userCartInfo != null)
            {
                userCartInfo.Items = null;
                userCartInfo.VendorDetails = null;

                var isSuccess = await _redisCacheBasketService.UpdateBasketItems(request.Username,userCartInfo);
                return isSuccess;
            }
            else
            {
                _logger.LogInformation("Username {0} is not present in database", request.Username);
                return false;
            }
        }
    }
}
