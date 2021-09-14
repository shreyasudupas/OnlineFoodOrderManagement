using MenuInventory.Microservice.Data.Context;
using MenuInventory.Microservice.Data.VendorCartModel;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MenuInventory.Microservice.Data.VendorCartConfigurationRepository
{
    public class VendorCartRepository:BaseRepository<VendorCartConfigurations>
    {
        public VendorCartRepository(IMongoDBContext mongoDBContext):base(mongoDBContext)
        {
        }

        public async Task<VendorCartConfigurations> GetVendorConfigurationByVendorId(string VendorId)
        {
            var filter = Builders<VendorCartConfigurations>.Filter.Eq(x => x.VendorDetails.VendorId, VendorId);

            var VendorCartConfigurationsReponse = await mongoCollection.Find(filter).FirstOrDefaultAsync();
            return VendorCartConfigurationsReponse;
        }
    }
}
