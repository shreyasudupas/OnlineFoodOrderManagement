using AutoMapper;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using MenuManagment.Domain.Mongo.Entities;
using MenuManagment.Domain.Mongo.Interfaces;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public class VendorCartRepository : BaseRepository<VendorCartConfigurations> , IVendorCartRepository
    {
        private readonly IMapper _mapper;

        public VendorCartRepository(IMongoDBContext mongoDBContext,
            IMapper mapper) : base(mongoDBContext)
        {
            _mapper = mapper;
        }

        public async Task<VendorCartDetails> GetVendorConfigurationByVendorId(string VendorId)
        {
            var filter = Builders<VendorCartConfigurations>.Filter.Eq(x => x.VendorDetails.VendorId, VendorId);

            var VendorCartConfigurationsReponse = await mongoCollection.Find(filter).FirstOrDefaultAsync();

            return _mapper.Map<VendorCartDetails>(VendorCartConfigurationsReponse);
        }
    }
}
