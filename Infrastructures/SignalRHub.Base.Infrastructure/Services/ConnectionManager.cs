using MenuManagement.SignalR.HubService.Common.Models;
using SignalRHub.Base.Infrastructure.Common.Interfaces.Hub;

namespace SignalRHub.Base.Infrastructure.Services;

public class ConnectionManager : IConnectionManager
{
    private readonly List<HubUser> _hubUsers;
    private readonly object _lockObj = new object();

    public ConnectionManager()
    {
        _hubUsers = new();
    }

    public void AddConnection(string userId, string role, string connectionId)
    {
        lock (_lockObj)
        {
            var user = _hubUsers.Find(x => x.UserId == userId);
            if (user == null)
            {
                _hubUsers.Add(new HubUser
                {
                    UserId = userId,
                    Role = role,
                    ConnectionId = connectionId
                });
            }
        }
    }

    public List<HubUser> GetAllUsersConnections()
    {
        return _hubUsers;
    }

    public HubUser? GetUserConnection(string userId)
    {
        var user = _hubUsers.Find(x => x.UserId == userId);
        if (user is not null)
        {
            return user;
        }

        return null;
    }

    public void RemoveConnection(string userId, string connectionId)
    {
        lock (_lockObj)
        {
            var user = _hubUsers.Find(x => x.UserId == userId && x.ConnectionId == connectionId);
            if (user != null)
            {
                _hubUsers.Remove(user);
            }
        }
    }
}

