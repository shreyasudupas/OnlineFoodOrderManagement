using Common.Mongo.Database.Data.BaseContext;
using Common.Mongo.Database.Data.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Common.Mongo.Database.Data.Context
{
    public class MenuRepository : BaseRepository<Menus>
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
    }
}
