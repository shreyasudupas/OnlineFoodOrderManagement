using Notification.Microservice.Core.Domain.Model;
using System.Collections.Generic;

namespace Notification.Microservice.Core.Hub
{
    public interface IConnectionMapping
    {
        void Add(string userId, string role, string connectionId);

        HubConnectionManager? GetConnection(string userId);

        List<HubConnectionManager> GetAllConnectionManager();

        void Remove(string userId, string connectionId);

    }
}
