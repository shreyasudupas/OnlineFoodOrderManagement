using Notification.Microservice.Core.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace Notification.Microservice.Core.Hub
{
    public class ConnectionMapping : IConnectionMapping
    {
        private readonly List<HubConnectionManager> connectionManagers;
        public ConnectionMapping()
        {
            connectionManagers = new List<HubConnectionManager>();
        }

        public void Add(string userId ,string role, string connectionId)
        {
            lock (connectionManagers)
            {
                var user = connectionManagers.Find(x => x.UserId == userId);
                if (user == null)
                {
                    connectionManagers.Add(new HubConnectionManager 
                    {
                        UserId = userId,
                        Role = role,
                        ConnectionId = connectionId
                    });
                }
            }
        }

        public HubConnectionManager? GetConnection(string userId)
        {
            var user = connectionManagers.Find(x => x.UserId == userId);
            if(user != null)
            {
                return user;
            }

            return null;
        }

        public List<HubConnectionManager> GetAllConnectionManager()
        {
            return connectionManagers;
        }

        public void Remove(string userId, string connectionId)
        {
            lock (connectionManagers)
            {
                var user = connectionManagers.Find(x => x.UserId == userId && x.ConnectionId == connectionId);
                if (user != null)
                {
                    connectionManagers.Remove(user);
                }
            }
        }
    }
}
