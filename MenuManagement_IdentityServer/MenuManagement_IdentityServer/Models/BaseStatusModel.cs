using System.Collections.Generic;

namespace MenuManagement_IdentityServer.Models
{
    public class BaseStatusModel
    {
        public BaseStatusModel()
        {
            ErrorDescription = new List<string>();
        }
        public List<string> ErrorDescription { get; set; }
        public CrudEnumStatus status { get; set; }
    }
}
