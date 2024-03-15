using System;

namespace MenuOrder.Shared.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string errorMessages) : base(errorMessages) 
    {
    }
}
