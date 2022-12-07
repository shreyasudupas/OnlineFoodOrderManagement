using AutoMapper;
using MenuManagement.Core.Common.Models.InventoryService;
using MenuManagement.Core.Mongo.Interfaces;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagement.Infrastructure.Persistance.MongoDatabase.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public class VendorRepository : BaseRepository<Vendor>, IVendorRepository
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public VendorRepository(IMongoDBContext mongoDBContext,
            ILogger<VendorRepository> logger,
            IMapper mapper
            ) : base(mongoDBContext)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<VendorDto> AddVendorDocument(VendorDto vendor)
        {
            _logger.LogInformation("AddVendorDocument started..");
            if (vendor != null)
            {
                var mapToVendorModel = _mapper.Map<Vendor>(vendor);

                await CreateOneDocument(mapToVendorModel);

                var getVendorWithId = await GetByFilter(v=>v.VendorName == vendor.VendorName);

                var vendorWithIdMapTo = _mapper.Map<VendorDto>(getVendorWithId);

                return vendorWithIdMapTo;
            }
            else
            {
                _logger.LogError("No Items present to add");
                return null;
            }
        }

        public async Task<List<VendorDto>> AddVendorDocuments(List<VendorDto> vendors)
        {
            _logger.LogInformation("AddVendorDocuments started..");
            if(vendors.Count > 0)
            {
                var mapToVendorModel = _mapper.Map<List<Vendor>>(vendors);

                await CreateManyDocument(mapToVendorModel);

                return vendors;
            }
            else
            {
                _logger.LogError("No Items present to add");
                return null;
            }
        }

        public async Task<List<VendorDto>> GetAllVendorDocuments()
        {
            _logger.LogInformation("GetAllVendorDocuments started..");

            var vendors = await GetAllItems();

            if(vendors.ToList().Count > 0)
            {
                var mapToVendorDto = _mapper.Map<List<VendorDto>>(vendors);

                return mapToVendorDto;
            }
            else
            {
                _logger.LogInformation("No Vendors is database");
                return new List<VendorDto>();
            }
        }

        public async Task<VendorDto> GetVendorDocument(string id)
        {
            _logger.LogInformation("GetVendorDocument started..");

            var vendor = await GetById(id);

            if (vendor != null)
            {
                var mapToVendorDto = _mapper.Map<VendorDto>(vendor);

                return mapToVendorDto;
            }
            else
            {
                _logger.LogInformation($"Vendors with id: {id} is database");
                return new VendorDto();
            }
        }

        public async Task<VendorDto> GetVendorDocumentByCustomerfilter(Expression<Func<VendorDto, bool>> filterExpression)
        {
            _logger.LogInformation("GetVendorDocumentByCustomerfilter started..");

            return await GetVendorDocumentByCustomerfilter(filterExpression);
        }

        public int IfVendorCollectionExists()
        {
            return IfDocumentExists();
        }
    }
}
