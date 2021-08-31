using Common.Utility.Models;
using System.Threading.Tasks;

namespace Common.Utility.BuisnessLayer.IBuisnessLayer
{
    public interface IUser
    {
        Task<Users> AddOrGetUserDetails(UserProfile userProfile);
    }
}
