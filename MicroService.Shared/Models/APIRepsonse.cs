using System;

namespace Common.Utility.Models
{
    public class APIResponse
    {
        public int Response { get; set; }
        public Object Content { get; set; }
        public Object Exception { get; set; }
    }
}
