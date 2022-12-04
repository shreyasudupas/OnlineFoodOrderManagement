using AutoMapper;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using MenuManagment.Domain.Mongo.Entities;
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

        Task<Core.Mongo.Entities.VendorCartDetails> IVendorCartRepository.GetVendorConfigurationByVendorId(string VendorId)
        {
            throw new System.NotImplementedException();
        }
    }
}
