using MongoDB.Driver;

namespace MongoDb.Infrastructure.Persistance.Persistance.MongoDatabase.DbContext
{
    public interface IMongoDBContext
    {
        IMongoCollection<Order> GetCollection<Order>(string name);
    }
}
