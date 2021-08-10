using MenuInventory.Microservice.Data.Context;
using MenuInventory.Microservice.Data.MenuModels;

namespace MenuInventory.Microservice.Data.MenuRepository
{
    public class MenuRepository:BaseRepository<Menus>
    {
        public MenuRepository(IMongoDBContext mongoDBContext):base(mongoDBContext)
        {
        }
    }
}
