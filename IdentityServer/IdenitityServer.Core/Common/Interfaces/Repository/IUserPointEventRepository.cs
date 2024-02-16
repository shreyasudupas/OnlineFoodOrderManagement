using IdenitityServer.Core.Domain.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdenitityServer.Core.Common.Interfaces.Repository
{
    public interface IUserPointEventRepository
    {
        Task<List<UserPointsEvent>> GetAllUserEvents(string userId);
        Task<UserPointsEvent> GetCurrentUserEvent(string userId);
        Task SaveUserEvent(UserPointsEvent userPointsEvent);
    }
}
