using MicroService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.MicroService.BuisnessLayer.IBuisnessLayer
{
    public interface ICartService
    {
        Task<int> AddItemsinCart(string Username,CartItems cartItems);
        Task<UserCartInfo> GetItemsFromCacheCart(string Username);

        Task<int> RemoveItemsFromCart(string Username, CartItems cartItems);
    }
}
