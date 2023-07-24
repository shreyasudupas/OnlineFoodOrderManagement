namespace OrderManagement.Microservice.Core.Common.Interfaces.CartInformation
{
    public interface ICartInformationRepository
    {
        Task<MenuManagment.Mongo.Domain.Entities.CartInformation> AddUserCartInformation
            (MenuManagment.Mongo.Domain.Entities.CartInformation cartInformation);

        Task<MenuManagment.Mongo.Domain.Entities.CartInformation> GetActiveUserCartInformation(string userId);

        Task<MenuManagment.Mongo.Domain.Entities.CartInformation?> UpdateCartInformation(MenuManagment.Mongo.Domain.Entities.CartInformation cartInformation);

        Task<bool> CheckIfMenuItemsBelongToSameVendor(string userId, string vendorId);
        Task<bool> CartActiveMenuItemsClear(string userId);
    }
}
