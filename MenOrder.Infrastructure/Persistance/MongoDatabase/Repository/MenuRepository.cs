using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using MenuManagment.Domain.Mongo.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public class MenuRepository : BaseRepository<Menus> , IMenuRepository
    {
        public MenuRepository(IMongoDBContext mongoDBContext) : base(mongoDBContext)
        {
        }

        public async Task<string> GetVendorDetails_DisplayName(string VendorId, string SearchColumnName)
        {
            var GetVendorMenuDetails = await mongoCollection.Find(x => x.Id == VendorId).FirstOrDefaultAsync();

            if (GetVendorMenuDetails != null)
            {
                var VendorColumnDetail = GetVendorMenuDetails.VendorDetails.ColumnDetails.Find(x => x.ColumnName == SearchColumnName);

                if (VendorColumnDetail != null)
                {
                    return VendorColumnDetail.DisplayName;
                }
                else
                    return string.Empty;
            }
            else
                return string.Empty;

        }

        public async Task<List<MenuManagment.Domain.Mongo.Entities.VendorDetail>> ListAllVendorDetails(string Locality)
        {
            var result = await mongoCollection.Find(x => x.VendorArea == Locality).ToListAsync();
            var modelList = new List<MenuManagment.Domain.Mongo.Entities.VendorDetail>();

            foreach(var item in result)
            {
                modelList.Add(new MenuManagment.Domain.Mongo.Entities.VendorDetail
                {
                    VendorId = item.Id,
                    Description = item.Description,
                    Location = item.Location,
                    Rating = item.Rating,
                    VendorName = item.VendorName
                });
            }
            return modelList;
        }
    }
}
