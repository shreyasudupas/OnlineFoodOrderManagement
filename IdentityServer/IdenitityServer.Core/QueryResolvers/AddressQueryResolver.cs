using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.QueryResolvers
{
    public class AddressQueryResolver
    {
        private readonly IAddressService _addressService;

        public AddressQueryResolver(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<List<State>> GetAddressList()
        {
            return await _addressService.GetAddressList();
        }
    }
}
