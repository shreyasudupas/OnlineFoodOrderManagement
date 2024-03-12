using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Enum;
using MenuManagment.Mongo.Domain.Mongo.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDb.Shared.Persistance.Repositories;
using MongoDB.Driver;
using OrderManagement.Microservice.Core.Common.Interfaces.CartInformation;

namespace OrderManagement.Mongo.Persistance.Repositories
{
    public class CartInformationRepository : BaseRepository<CartInformation> , ICartInformationRepository
    {
        private readonly ILogger _logger;

        public CartInformationRepository(ILogger<CartInformationRepository> logger,
            IOptions<MongoDatabaseConfiguration> mongoDatabaseSettings
            )
            : base(mongoDatabaseSettings)
        {
            _logger = logger;
        }

        public async Task<CartInformation> AddUserCartInformation(CartInformation cartInformation)
        {
            var result = new CartInformation();
            _logger.LogInformation("Add User Cart Information started");

            await AssignIndexForCartInformation();

            //Here there is only one active cart Information present for one user
            result = await GetDocumentByFilter(c => c.UserId == cartInformation.UserId && c.CartStatus == nameof(CartStatusEnum.Active));

            if(result == null)
            {
                await CreateOneDocument(cartInformation);

                result = await GetDocumentByFilter(c => c.UserId == cartInformation.UserId && c.CartStatus == nameof(CartStatusEnum.Active));
            }
            else
            {
                _logger.LogInformation($"User Cart Information for userid:{cartInformation.UserId} is already present");
            }

            _logger.LogInformation("Add User Cart Information ended");
            return result;
        }

        public async Task<CartInformation> GetActiveUserCartInformation(string userId)
        {
            _logger.LogInformation("Get User CartInformation started");

            //await AssignIndexForCartInformation();

            //Here there is only one active cart Information present for one user
            var result = await GetDocumentByFilter(c => c.UserId == userId && c.CartStatus == nameof(CartStatusEnum.Active));

            _logger.LogInformation("Get User CartInformation ended");

            return result;
        }

        public async Task AssignIndexForCartInformation()
        {
            var indexKeyDefinitionBuilder = Builders<CartInformation>.IndexKeys;
            var indexModel = new CreateIndexModel<CartInformation>
                (indexKeyDefinitionBuilder.Ascending(indexKey => indexKey.UserId));
            var result = await CreateOneIndexAsync(indexModel);
        }

        public async Task<CartInformation?> UpdateCartInformation(CartInformation cartInformation)
        {
            var isCartInfo = await GetDocumentByFilter(c => c.UserId == cartInformation.UserId && c.CartStatus == nameof(CartStatusEnum.Active));
            if(isCartInfo is not null)
            {
                var filter = Builders<CartInformation>.Filter.Eq(x => x.Id, cartInformation.Id);
                var update = Builders<CartInformation>.Update.Set(m => m.MenuItems, cartInformation.MenuItems);

                var updateResult = await UpdateOneDocument(filter, update);
                if(updateResult.IsAcknowledged)
                {
                    var updatedCartInfo = await GetDocumentByFilter(c => c.UserId == cartInformation.UserId && c.CartStatus == nameof(CartStatusEnum.Active));

                    return updatedCartInfo;
                }
                else
                {
                    _logger.LogError($"Cart with userId: {cartInformation.UserId} has encountred an issue when updating");
                    return cartInformation;
                }
            }

            return isCartInfo;
        }

        public async Task<bool> CheckIfMenuItemsBelongToSameVendor(string userId,string vendorId)
        {
            var filter = Builders<CartInformation>.Filter.Where(v => v.UserId == userId && v.CartStatus == nameof(CartStatusEnum.Active));
            var cartMenuItemPresent = await mongoCollection.Find(filter).FirstOrDefaultAsync();

            if(cartMenuItemPresent.MenuItems.Any())
            {
                var filter2 = Builders<CartInformation>.Filter.Where(v => v.UserId == userId && v.MenuItems.Any(m => m.VendorId == vendorId) && v.CartStatus == nameof(CartStatusEnum.Active));
                var cartIfPresent = await mongoCollection.Find(filter2).FirstOrDefaultAsync();

                if (cartIfPresent != null)
                {
                    _logger.LogInformation($"Menu Item with VendorId: {vendorId} is present in the Cart");
                    return true;
                }
                else
                {
                    _logger.LogInformation($"Menu Item with VendorId: {vendorId} is not present in the Cart");
                    return false;
                }
            }
            else
            {
                //that means cart menu Items are empty and any vendor menu item can be added
                return true;
            }
        }

        public async Task<bool> CartActiveMenuItemsClear(string userId)
        {
            var cartItems = await GetDocumentByFilter(c => c.UserId == userId && c.CartStatus == nameof(CartStatusEnum.Active));

            if(cartItems is not null)
            {
                var filter = Builders<CartInformation>.Filter.Where(x => x.UserId == userId && x.CartStatus == nameof(CartStatusEnum.Active));
                var update = Builders<CartInformation>.Update.Set(c => c.MenuItems, new List<MenuItem>());

                var result = await UpdateOneDocument(filter, update);
                if(result.IsAcknowledged)
                {
                    return true;
                }

                _logger.LogError($"Unable to update Clear Cart Operation for userid:{userId}");
                return false;
            }
            else
            {
                _logger.LogError($"Given the UserId {userId} for the Cart is not Active");
                return false;
            }
        }
    }
}
