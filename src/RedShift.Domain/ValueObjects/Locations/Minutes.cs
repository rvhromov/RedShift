using RedShift.Domain.Exceptions;

namespace RedShift.Domain.ValueObjects.Locations;

public sealed record Minutes
{
    private const short MinMinutesValue = 0;
    private const short MaxMinutesValue = 60;

    public Minutes(short value)
    {
        if (value < MinMinutesValue || value > MaxMinutesValue)
        {
            throw new ValidationException($"Invalid minutes value. The minutes must be in range between {MinMinutesValue} and {MaxMinutesValue}.");
        }

        Value = value;
    }

    public short Value { get; }

    public static implicit operator Minutes(short minutes) =>
        new(minutes);

    public static implicit operator short(Minutes minutes) =>
        minutes.Value;
}
