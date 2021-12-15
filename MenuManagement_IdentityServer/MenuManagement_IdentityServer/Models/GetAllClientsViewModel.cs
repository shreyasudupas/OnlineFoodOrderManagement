using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class GetAllClientsViewModel : BaseStatusModel
    {
        public GetAllClientsViewModel()
        {
            ServiceData = new DisplayAllClients();
        }
        public DisplayAllClients ServiceData { get; set; }
    }
}
