using MongoDB.Driver;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext
{
    public interface IMongoDBContext
    {
        IMongoCollection<Order> GetCollection<Order>(string name);
    }
}
