﻿using MenuManagement.Core.Mongo.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuManagment.Mongo.Domain.Mongo.Interfaces
{
    public interface IVendorRepository
    {
        Task<List<VendorDto>> AddVendorDocuments(List<VendorDto> vendors);
        Task<List<VendorDto>> GetAllVendorDocuments();
    }
}
