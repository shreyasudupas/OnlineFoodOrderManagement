using MenuManagment.Mongo.Domain.Entities;
using MenuManagment.Mongo.Domain.Enum;
using MenuManagment.Mongo.Domain.Mongo.Entities;
using Microsoft.Extensions.Logging;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Repositories;
using MongoDB.Driver;
using OrderManagement.Microservice.Core.Common.Interfaces.CartInformation;

namespace OrderManagement.Mongo.Persistance.Repositories
{
    public class CartInformationRepository : BaseRepository<CartInformation> , ICartInformationRepository
    {
        private readonly ILogger _logger;

        public CartInformationRepository(ILogger<CartInformationRepository> logger,
            IMongoDBContext mongoDBContext)
            : base(mongoDBContext)
        {
            _logger = logger;
        }

        public async Task<CartInformation> AddUserCartInformation(CartInformation cartInformation)
        {
            var result = new CartInformation();
            _logger.LogInformation("Add User Cart Information started");

            await AssignIndexForCartInformation();

            //Here there is only one active cart Information present for one user
            result = await GetByFilter(c => c.UserId == cartInformation.UserId && c.CartStatus == nameof(CartStatusEnum.Active));

            if(result == null)
            {
                await CreateOneDocument(cartInformation);

                result = await GetByFilter(c => c.UserId == cartInformation.UserId && c.CartStatus == nameof(CartStatusEnum.Active));
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
            var result = await GetByFilter(c => c.UserId == userId && c.CartStatus == nameof(CartStatusEnum.Active));

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
            var isCartInfo = await GetByFilter(c => c.UserId == cartInformation.UserId && c.CartStatus == nameof(CartStatusEnum.Active));
            if(isCartInfo is not null)
            {
                var filter = Builders<CartInformation>.Filter.Eq(x => x.Id, cartInformation.Id);
                var update = Builders<CartInformation>.Update.Set(m => m.MenuItems, cartInformation.MenuItems);

                var updateResult = await UpdateOneDocument(filter, update);
                if(updateResult.IsAcknowledged)
                {
                    var updatedCartInfo = await GetByFilter(c => c.UserId == cartInformation.UserId && c.CartStatus == nameof(CartStatusEnum.Active));

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
    }
}
