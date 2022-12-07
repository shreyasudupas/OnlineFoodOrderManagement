﻿using MenuManagement.Core.Common.Models.InventoryService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MenuManagement.Core.Mongo.Interfaces
{
    public interface IVendorRepository
    {
        Task<VendorDto> AddVendorDocument(VendorDto vendor);
        Task<List<VendorDto>> AddVendorDocuments(List<VendorDto> vendors);
        Task<List<VendorDto>> GetAllVendorDocuments();
        int IfVendorCollectionExists();

        Task<VendorDto> GetVendorDocument(string id);
        Task<VendorDto> GetVendorDocumentByCustomerfilter(Expression<Func<VendorDto, bool>> filterExpression);
    }
}
