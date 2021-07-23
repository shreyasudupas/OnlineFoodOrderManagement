using MenuOrder.MicroService.Data.Context;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MenuOrder.MicroService.Data
{
    public class OrderRepository:BaseRepository<Orders>
    {
       
        public OrderRepository(IMongoDBContext mongoDBContext):base(mongoDBContext)
        {
        }
        
        public async Task UpdateOrders(Orders orders)
        {
            //var OldData = await mongoCollection.Find(x => x.Id == orders.Id).FirstOrDefaultAsync();
            var Filter = Builders<Orders>.Filter.Eq(x => x.Id, orders.Id);
            //OldData.OrderItems.AddRange(orders.OrderItems);
            var updateRow = Builders<Orders>.Update.Set(x => x.OrderItems, orders.OrderItems);

            var update = await mongoCollection.UpdateOneAsync(Filter, updateRow);
            
        }

        public async Task<Orders> GetUserOrdersBasedOnUserName(string UserName)
        {
            var result = await mongoCollection.Find(x => x.UserInfo.UserName == UserName).FirstOrDefaultAsync();
            return result;
        }

        public async Task InsertUserOrders(Orders orders)
        {
           await mongoCollection.InsertOneAsync(orders);
        }
    }
}
