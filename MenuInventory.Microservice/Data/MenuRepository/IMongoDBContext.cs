using MongoDB.Driver;

namespace MenuInventory.Microservice.Data.Context
{
    public interface IMongoDBContext
    {
        IMongoCollection<Order> GetCollection<Order>(string name);
    }
}
