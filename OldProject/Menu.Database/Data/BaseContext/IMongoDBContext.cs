using MongoDB.Driver;

namespace Common.Mongo.Database.Data.BaseContext
{
    public interface IMongoDBContext
    {
        IMongoCollection<Order> GetCollection<Order>(string name);
    }
}
