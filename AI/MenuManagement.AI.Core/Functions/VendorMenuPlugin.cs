using MenuManagement.AI.Domain.Models;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace MenuManagement.AI.Core.Functions;

public class VendorMenuPlugin(
    IVendorRepository vendorRepository,
    IVendorsMenuRepository vendorsMenuRepository
    )
{
    public UserCartModel _userCart = new();

    [KernelFunction("get_all_restaurants")]
    [Description("Gets list of restaurants")]
    [return: Description("An array of restaurents")]
    public async Task<List<RestaurentModel>> GetAllVendors()
    {
        List<RestaurentModel> restaurentList = new();
        var vendorList = await vendorRepository.GetAllVendorDocuments();

        if(vendorList is not null)
        {
            vendorList.ForEach(vendor =>
            {
                restaurentList.Add(new RestaurentModel
                {
                    RestaurentId = vendor.Id,
                    RestaurantName = vendor.VendorName,
                    RestaurantDescription = vendor.VendorDescription,
                    Rating = vendor.Rating,
                    Area = vendor.Area,
                    City = vendor.City,
                });
            });
        }
        var res = restaurentList.TakeLast(5);
        return res.ToList();
    }

    [KernelFunction("get_restaurant_menu")]
    [Description("Gets the menu list of the restaurant by passing the restaurent Id")]
    [return: Description("An array of menu item")]
    public async Task<List<MenuItemModel>> GetVendorMenuItems(
        [Description("restaurent Id")]
        string vendorId
        )
    {
        List<MenuItemModel> menuItems = new();
        var menuList = await vendorsMenuRepository.GetAllVendorMenuByVendorId(vendorId);

        if (menuList.Count > 0)
        {
            menuList.ForEach((menu) =>
            {
                menuItems.Add(new MenuItemModel
                {
                    MenuId = menu.Id,
                    RestaurentId = menu.VendorId,
                    FoodType = menu.FoodType,
                    MenuItemName = menu.ItemName,
                    Price = menu.Price,
                    Rating = menu.Rating
                });
            });
        }
        return menuItems;
    }

    [KernelFunction("user_menu_cart")]
    [Description("add or updates or removes the list of menu item in the users cart;")]
    public void UserCartInformation(
        [Description("deleted or removed menu items in the cart")]
        List<MenuCartModel> removedMenuItems,
        [Description("add or updated quantity of menu items in the cart")]
        List<MenuCartModel> updatedMenuItems
        
        )
    {
        //Remove or Fully delete item
        foreach(var removedMenuItem in removedMenuItems)
        {
            if (_userCart.CartInformations.TryGetValue(removedMenuItem.MenuId, out var existingMenuItem))
            {
                if(removedMenuItem.Quantity <= 1 )
                {
                    _userCart.CartInformations.Remove(existingMenuItem.MenuId);
                }
                else
                {
                    existingMenuItem.Quantity = -removedMenuItem.Quantity;
                }
            }
        }

        //Add or Increase menu item
        foreach(var updatedMenuItem in updatedMenuItems)
        {
            if (_userCart.CartInformations.TryGetValue(updatedMenuItem.MenuId, out var existingMenuItem))
            {
                existingMenuItem.Quantity = updatedMenuItem.Quantity;
            }
            else
            {
                _userCart.CartInformations.Add(updatedMenuItem.MenuId, new MenuCartModel
                {
                    MenuId = updatedMenuItem.MenuId,
                    MenuItemName = updatedMenuItem.MenuItemName,
                    Quantity = updatedMenuItem.Quantity,
                });
            }
        }
    }

    [KernelFunction("get_user_cart")]
    [Description("gets array of menu items from users cart")]
    [return:Description("An array of MenuCart Model")]
    public List<MenuCartModel> GetUserCartMenuItems()
    {
        var userCart = _userCart.CartInformations.Select(x => new MenuCartModel
        {
            MenuId = x.Value.MenuId,
            MenuItemName = x.Value.MenuItemName,
            Quantity = x.Value.Quantity,
        }).ToList();

        return userCart;
    }
}