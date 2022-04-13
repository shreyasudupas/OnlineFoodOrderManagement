using MenuManagement.Core.Common.Models.BasketService;
using System.Threading.Tasks;

namespace MenuManagement.Core.Common.Interfaces
{
    public interface IRedisCacheBasketService
    {
        Task<UserCartInformation> GetBasketItems(string Username);
        Task<bool> UpdateBasketItems(string Username, UserCartInformation userCartInformation);
    }
}
