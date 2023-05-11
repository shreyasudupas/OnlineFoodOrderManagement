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
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger _logger;

        public AddressService(ApplicationDbContext applicationDbContext,
            ILogger<AddressService> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<List<State>> GetAddressList()
        {
            var address = await _applicationDbContext.States.Include(x=>x.Cities).ThenInclude(x=>x.Areas).ToListAsync();

            return address;
        }

        public async Task<State> AddState(State newState)
        {
            var isState = await _applicationDbContext.States.AnyAsync(s => s.Name == newState.Name);
            if(!isState)
            {
                _applicationDbContext.States.Add(newState);
                _applicationDbContext.SaveChanges();
                return _applicationDbContext.States.Where(s=>s.Name == newState.Name).FirstOrDefault();
            }else
            {
                _logger.LogError($"State with {newState.Name} already exists");
                return null;
            }
        }

        public async Task<State> DeleteState(State state)
        {
            var isState = await _applicationDbContext.States.Include(x=>x.Cities).AnyAsync(s => s.Id == state.Id);
            if (isState)
            {
                _applicationDbContext.States.Remove(state);
                _applicationDbContext.SaveChanges();
                return state;
            }
            else
            {
                _logger.LogError($"State with Name: {state.Name} with Id: {state.Id} not present");
                return null;
            }
        }

        public async Task<City> AddCity(City newCity)
        {
            var isCity = await _applicationDbContext.Cities.AnyAsync(s => s.Name == newCity.Name && s.StateId == newCity.StateId);
            if (!isCity)
            {
                _applicationDbContext.Cities.Add(newCity);
                _applicationDbContext.SaveChanges();
                return _applicationDbContext.Cities.Where(x=>x.StateId==newCity.StateId && x.Name == newCity.Name).FirstOrDefault();
            }
            else
            {
                _logger.LogError($"City with {newCity.Name} already exists");
                return null;
            }
        }

        public async Task<City> DeleteCity(City city)
        {
            var isCity = await _applicationDbContext.Cities.Include(x=>x.Areas).AnyAsync(c => c.Id == city.Id && c.StateId == city.StateId);
            if (isCity)
            {
                _applicationDbContext.Cities.Remove(city);
                _applicationDbContext.SaveChanges();
                return city;
            }
            else
            {
                _logger.LogError($"City with Name: {city.Name} with Id: {city.Id} and StateId:{city.StateId} not present");
                return null;
            }
        }

        public async Task<LocationArea> AddArea(LocationArea newArea)
        {
            var isArea = await _applicationDbContext.LocationAreas.AnyAsync(a => a.AreaName == newArea.AreaName
                && a.CityId == newArea.CityId);
            if (!isArea)
            {
                _applicationDbContext.LocationAreas.Add(newArea);
                _applicationDbContext.SaveChanges();
                return _applicationDbContext.LocationAreas.Where(x => x.CityId == newArea.CityId && x.AreaName == newArea.AreaName).FirstOrDefault();
            }
            else
            {
                _logger.LogError($"Area with {newArea.AreaName} already exists");
                return null;
            }
        }

        public async Task<LocationArea> DeleteArea(LocationArea area)
        {
            var isArea = await _applicationDbContext.LocationAreas.AnyAsync(c => c.Id == area.Id && c.CityId == area.CityId);
            if (isArea)
            {
                _applicationDbContext.LocationAreas.Remove(area);
                _applicationDbContext.SaveChanges();
                return area;
            }
            else
            {
                _logger.LogError($"Area with Name: {area.AreaName} with Id: {area.Id} and CityId:{area.CityId} not present");
                return null;
            }
        }

        public async Task<string> GetCityNameById(int cityId)
        {
            var cityName = await _applicationDbContext.Cities.Where(c => c.Id == cityId).Select(c => c.Name).FirstOrDefaultAsync();
            return cityName;
        }

        public async Task<string> GetStateNameById(int stateId)
        {
            var cityName = await _applicationDbContext.States.Where(s => s.Id == stateId).Select(s => s.Name).FirstOrDefaultAsync();
            return cityName;
        }

        public async Task<string> GetAreaNameById(int areaId)
        {
            var cityName = await _applicationDbContext.LocationAreas.Where(a => a.Id == areaId).Select(a => a.AreaName).FirstOrDefaultAsync();
            return cityName;
        }
    }
}
