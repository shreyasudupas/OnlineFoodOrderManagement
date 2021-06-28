using System;

namespace MicroService.Shared.Models
{
    public class APIResponse
    {
        public int Response { get; set; }
        public Object Content { get; set; }
        public Object Exception { get; set; }
    }
}
