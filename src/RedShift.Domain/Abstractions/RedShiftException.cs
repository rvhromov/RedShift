using RedShift.Domain.Enums;

namespace RedShift.Domain.Abstractions;

public abstract class RedShiftException : Exception
{
    protected RedShiftException(ExceptionType type, string message) : base(message) 
    { 
        Type = type;
    }

    public ExceptionType Type { get; private set; }
}
