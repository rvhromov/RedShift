using RedShift.Domain.Abstractions;
using RedShift.Domain.Enums;

namespace RedShift.Domain.Exceptions;

public sealed class ValidationException : RedShiftException
{
    public ValidationException(string message) : base(ExceptionType.Validation, message)
    {
    }
}
