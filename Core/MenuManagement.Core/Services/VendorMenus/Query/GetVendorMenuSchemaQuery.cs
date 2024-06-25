using Inventory.Microservice.Core.Common.SchemaGenerator;
using MediatR;
using MenuManagment.Mongo.Domain.Mongo.Inventory.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Microservice.Core.Services.VendorMenus.Query
{
    public class GetVendorMenuSchemaQuery : IRequest<string>
    {
    }

    public class GetVendorMenuSchemaQueryHandler : IRequestHandler<GetVendorMenuSchemaQuery, string>
    {
        public Task<string> Handle(GetVendorMenuSchemaQuery request, CancellationToken cancellationToken)
        {
            var schema = JsonSchmeaGeneratorFunction.GetSchema<VendorMenuDto[]>();
            return Task.FromResult(schema.ToString());
        }
    }
}
