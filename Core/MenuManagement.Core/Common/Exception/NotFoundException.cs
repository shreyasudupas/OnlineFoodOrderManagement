using System;

namespace MenuManagement.Core.Common.Exception
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"{name} ({key} not found)")
        {
        }
    }
}
