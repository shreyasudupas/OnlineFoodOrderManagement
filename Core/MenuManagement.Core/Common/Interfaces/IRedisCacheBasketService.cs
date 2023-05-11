using Inventory.Microservice.Core.Common.Models.BasketService;
using Inventory.Microservice.Core.Common.Models.Common;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Common.Interfaces
{
    public interface IRedisCacheBasketService
    {
        Task<UserCartInformation> GetBasketItems(string Username);
        Task<bool> UpdateBasketItems(string Username, UserCartInformation userCartInformation);

        Task<bool> AddUserInformationInBasket(UserInfoCart userInformation);
    }
}
