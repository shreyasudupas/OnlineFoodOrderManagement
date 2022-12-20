using IdenitityServer.Core.Common.Interfaces;
using IdenitityServer.Core.Domain.DBModel;
using System.Threading.Tasks;

namespace IdenitityServer.Core.MutationResolver
{
    public class AddressMutationResolver
    {
        private readonly IAddressService _addressService;

        public AddressMutationResolver(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<State> AddState(State newState)
        {
            return await _addressService.AddState(newState);
        }

        public async Task<State> DeleteState(State state)
        {
            return await _addressService.DeleteState(state);
        }

        public async Task<City> AddCity(City newCity)
        {
            return await _addressService.AddCity(newCity);
        }

        public async Task<City> DeleteCity(City city)
        {
            return await _addressService.DeleteCity(city);
        }

        public async Task<LocationArea> AddArea(LocationArea newArea)
        {
            return await _addressService.AddArea(newArea);
        }

        public async Task<LocationArea> DeleteArea(LocationArea area)
        {
            return await _addressService.DeleteArea(area);
        }
    }
}
