using HotChocolate;
using HotChocolate.Types;
using IdenitityServer.Core.Domain.DBModel;
using IdenitityServer.Core.MutationResolver;
using System.Threading.Tasks;

namespace IdentityServer.API.GraphQL.Mutation
{
    [ExtendObjectType("Mutation")]
    public class AddressExtensionType
    {
        public async Task<State> AddState(State newState,
            [Service] AddressMutationResolver addressMutationResolver)
        {
            return await addressMutationResolver.AddState(newState);
        }

        public async Task<State> DeleteState(State state,
            [Service] AddressMutationResolver addressMutationResolver)
        {
            return await addressMutationResolver.DeleteState(state);
        }

        public async Task<City> AddCity(City newCity,
            [Service] AddressMutationResolver addressMutationResolver)
        {
            return await addressMutationResolver.AddCity(newCity);
        }

        public async Task<City> DeleteCity(City city,
            [Service] AddressMutationResolver addressMutationResolver)
        {
            return await addressMutationResolver.DeleteCity(city);
        }

        public async Task<LocationArea> AddArea(LocationArea newArea
            , [Service] AddressMutationResolver addressMutationResolver)
        {
            return await addressMutationResolver.AddArea(newArea);
        }

        public async Task<LocationArea> DeleteArea(LocationArea area
            ,[Service] AddressMutationResolver addressMutationResolver)
        {
            return await addressMutationResolver.DeleteArea(area);
        }
    }
}
