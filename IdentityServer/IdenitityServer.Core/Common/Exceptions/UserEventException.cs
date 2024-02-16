using System;

namespace IdenitityServer.Core.Common.Exceptions
{
    public class UserEventException : Exception
    {
        public UserEventException(string errorMessage) : base(errorMessage) 
        { 
        }
    }
}
