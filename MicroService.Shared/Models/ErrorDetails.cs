using System;
using System.Text.Json;

namespace MicroService.Shared.Models
{
    public class ErrorDetails
    {
        public int Response { get; set; }
        public Object Content { get; set; }
        public Object Exception { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
