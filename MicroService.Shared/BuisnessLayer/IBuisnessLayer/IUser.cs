using MicroService.Shared.Models;
using System.Threading.Tasks;

namespace MicroService.Shared.BuisnessLayer.IBuisnessLayer
{
    public interface IUser
    {
        Task<Users> AddOrGetUserDetails(UserProfile userProfile);
    }
}
