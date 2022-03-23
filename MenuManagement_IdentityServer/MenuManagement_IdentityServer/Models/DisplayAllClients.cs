using IdentityServer4.EntityFramework.Entities;
using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class DisplayAllClients : BaseStatusModel
    {
        public DisplayAllClients()
        {
            Data = new List<ClientData>();
        }
        public List<ClientData> Data { get; set; }

    }

    public class ClientData
    {
        public ClientData()
        {
            AllowedGrantTypes = new List<ClientGrantType>();
        }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public List<ClientGrantType> AllowedGrantTypes { get; set; }
    }
}
