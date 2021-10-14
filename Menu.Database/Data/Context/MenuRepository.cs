using Common.Mongo.Database.Data.BaseContext;
using Common.Mongo.Database.Data.Models;

namespace Common.Mongo.Database.Data.Context
{
    public class MenuRepository : BaseRepository<Menus>
    {
        public MenuRepository(IMongoDBContext mongoDBContext) : base(mongoDBContext)
        {
        }
    }
}
