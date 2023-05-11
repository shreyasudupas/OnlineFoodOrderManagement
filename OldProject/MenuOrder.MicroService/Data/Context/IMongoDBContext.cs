using MongoDB.Driver;

namespace MenuOrder.MicroService.Data.Context
{
    public interface IMongoDBContext
    {
        IMongoCollection<Order> GetCollection<Order>(string name);
    }
}
