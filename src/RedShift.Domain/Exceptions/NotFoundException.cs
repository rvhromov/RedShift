using RedShift.Domain.Abstractions;
using RedShift.Domain.Enums;

namespace RedShift.Domain.Exceptions;

public sealed class NotFoundException : RedShiftException
{
    public NotFoundException(string message) : base(ExceptionType.NotFound, message)
    {
    }
}
