using System;

namespace MenuManagement_IdentityServer.Models
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public Object Content { get; set; }
        public Object Exception { get; set; }
    }
}
