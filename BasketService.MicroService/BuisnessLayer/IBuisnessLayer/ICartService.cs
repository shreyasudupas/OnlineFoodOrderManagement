using BasketService.MicroService.Models.GetCartItem;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BasketService.MicroService.BuisnessLayer.IBuisnessLayer
{
    public interface ICartService
    {
        //Task<int> AddItemsinCart(string Username,CartItems cartItems);
        //Task<UserCartInfo> GetItemsFromCacheCart(string Username);

        //Task<int> RemoveItemsFromCart(string Username, CartItems cartItems);

        Task<bool> AddItemsInCartV2(string Username, JObject CartInfoFromHeader);
        Task<bool> RemoveItemsFromCartV2(string Username, JObject CartInfoFromHeader);
        Task<string> GetAllBasketUserMenuList(string Username);
        Task<GetCartItemResponse> GetCartItemCount(string Username);
    }
}
