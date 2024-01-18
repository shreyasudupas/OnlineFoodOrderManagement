using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.Domain.Response;
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

        public async Task<List<VendorMappingResponse>> GetVendorUserMapping(string VendorId)
        {
            //var vendorUserList = await _applicationDbContext.VendorUserIdMappings
            //    .Where(v => v.VendorId == VendorId).ToListAsync();
            var query = (from vendorUserMapping in _applicationDbContext.VendorUserIdMappings
                         join userInfo in _applicationDbContext.Users
                        on vendorUserMapping.UserId equals userInfo.Id into VendorUserGroup
                         from userEnable in VendorUserGroup.DefaultIfEmpty()
                         where vendorUserMapping.VendorId == VendorId
                         select new VendorMappingResponse
                         {
                             Id = vendorUserMapping.Id,
                             EmailId = vendorUserMapping.EmailId,
                             Enabled = (userEnable == null) ? false : userEnable.Enabled,
                             UserId = vendorUserMapping.UserId,
                             Username = vendorUserMapping.Username,
                             UserType = vendorUserMapping.UserType,
                             VendorId = vendorUserMapping.VendorId
                         });
            var vendorUserList = await query.ToListAsync();

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

        public async Task<VendorUserIdMapping> UpdateVendorUserIdMapping(VendorUserIdMapping vendorUserIdMapping,bool enabled)
        {
            var getVendorUserMapping = await _applicationDbContext.VendorUserIdMappings
                .Where(v => v.Id == vendorUserIdMapping.Id)
                .FirstOrDefaultAsync();

            if(getVendorUserMapping != null)
            {
                getVendorUserMapping.UserId = vendorUserIdMapping.UserId;
                getVendorUserMapping.VendorId = vendorUserIdMapping.VendorId;
                getVendorUserMapping.EmailId = vendorUserIdMapping.EmailId;
                getVendorUserMapping.Username = vendorUserIdMapping.Username;

                var userProfile = await _applicationDbContext.Users.Where(x => x.Id == vendorUserIdMapping.UserId)
                                .FirstOrDefaultAsync();
                userProfile.Enabled = enabled;

                await _applicationDbContext.SaveChangesAsync();
            }

            return vendorUserIdMapping;
        }

        public async Task<VendorUserIdMapping> GetVendorUserMappingBasedOnEmailId(string emailId)
        {
            var vendorUserMapping = await _applicationDbContext.VendorUserIdMappings
                .Where(v => v.EmailId == emailId)
                .FirstOrDefaultAsync();
            return vendorUserMapping;
        }

        public async Task<VendorUserIdMapping> GetVendorUserMapping(string userId,string vendorId)
        {
            var vendorUserMapping = await _applicationDbContext.VendorUserIdMappings
                .Where(v => v.UserId == userId && v.VendorId == vendorId)
                .FirstOrDefaultAsync();

            return vendorUserMapping;
        }
    }
}
