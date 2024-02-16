using System;

namespace IdenitityServer.Core.Common.Exceptions
{
    public class ApplicationDbContextException : Exception
    {
        public ApplicationDbContextException(string message, Exception innerException) 
            : base(message,innerException)
        {
        }
    }
}
