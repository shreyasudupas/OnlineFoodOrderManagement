using System;
using System.Text.Json;

namespace MicroService.Shared.Models
{
    public class ErrorDetails
    {
        public int Response { get; set; }
        public Object Content { get; set; }
        public ExceptionDetails Exception { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class ExceptionDetails
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
