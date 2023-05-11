using IdenitityServer.Core.Domain.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces
{
    public interface IAddressService
    {
        Task<List<State>> GetAddressList();
        Task<State> AddState(State newState);
        Task<State> DeleteState(State state);
        Task<City> AddCity(City newCity);
        Task<City> DeleteCity(City city);
        Task<LocationArea> AddArea(LocationArea newArea);
        Task<LocationArea> DeleteArea(LocationArea area);
        Task<string> GetCityNameById(int cityId);
        Task<string> GetStateNameById(int stateId);
        Task<string> GetAreaNameById(int areaId);
    }
}
