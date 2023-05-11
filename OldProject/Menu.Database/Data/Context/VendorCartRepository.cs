using Common.Mongo.Database.Data.BaseContext;
using Common.Mongo.Database.Data.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Common.Mongo.Database.Data.Context
{
    public class VendorCartRepository : BaseRepository<VendorCartConfigurations>
    {
        public VendorCartRepository(IMongoDBContext mongoDBContext) : base(mongoDBContext)
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
