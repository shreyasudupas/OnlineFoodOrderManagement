using MongoDB.Driver;

namespace MongoDb.Shared.Persistance.DBContext
{
    public interface IMongoDBContext
    {
        IMongoCollection<Order> GetCollection<Order>(string name);
    }
}
