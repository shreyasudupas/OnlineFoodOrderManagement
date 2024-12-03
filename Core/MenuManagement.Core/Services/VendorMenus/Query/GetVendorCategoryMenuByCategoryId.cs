using MediatR;
using MenuManagment.Mongo.Domain.Dtos.Inventory;
using MenuManagment.Mongo.Domain.Mongo.Interfaces.Inventory.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.VendorMenus.Query;

public class GetVendorCategoryMenuByCategoryId : IRequest<VendorCategoryMenu>
{
    public string VendorId { get; set; }
    public string CategoryId { get; set; }
}

public class GetVendorCategoryMenuByCategoryIdHandler : IRequestHandler<GetVendorCategoryMenuByCategoryId, VendorCategoryMenu>
{
    private readonly IVendorRepository _vendorRepository;

    public GetVendorCategoryMenuByCategoryIdHandler(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<VendorCategoryMenu> Handle(GetVendorCategoryMenuByCategoryId request, CancellationToken cancellationToken)
    {
        return await _vendorRepository.GetVendorMenuListWithCategoryIdAsync(request.VendorId,
            request.CategoryId,
            cancellationToken);
    }
}
