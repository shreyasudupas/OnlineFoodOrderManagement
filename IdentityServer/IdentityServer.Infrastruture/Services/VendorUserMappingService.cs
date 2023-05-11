using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdentityServer.Infrastruture.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Infrastruture.Services
{
    public class VendorUserMappingService : IVendorUserMappingService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger _logger;

        public VendorUserMappingService(ApplicationDbContext applicationDbContext,
            ILogger<VendorUserMappingService> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<List<VendorUserIdMapping>> GetVendorUserMapping(string VendorId)
        {
            var vendorUserList = await _applicationDbContext.VendorUserIdMappings
                .Where(v => v.VendorId == VendorId).ToListAsync();

            return vendorUserList;
        }

        public async Task<VendorUserIdMapping> AddVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping)
        {
            if(vendorUserIdMapping != null)
            {
                await _applicationDbContext.VendorUserIdMappings.AddAsync(vendorUserIdMapping);
                await _applicationDbContext.SaveChangesAsync();
            }

            return vendorUserIdMapping;
        }

        public async Task<VendorUserIdMapping> UpdateVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping)
        {
            var getVendorUserMapping = await _applicationDbContext.VendorUserIdMappings
                .Where(v => v.Id == vendorUserIdMapping.Id).FirstOrDefaultAsync();

            if(getVendorUserMapping != null)
            {
                getVendorUserMapping.Enabled = vendorUserIdMapping.Enabled;
                getVendorUserMapping.UserId = vendorUserIdMapping.UserId;
                getVendorUserMapping.VendorId = vendorUserIdMapping.VendorId;

                await _applicationDbContext.SaveChangesAsync();
            }

            return vendorUserIdMapping;
        }
    }
}
