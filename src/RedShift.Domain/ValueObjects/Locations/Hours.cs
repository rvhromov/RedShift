using RedShift.Domain.Exceptions;

namespace RedShift.Domain.ValueObjects.Locations;

public sealed record Hours
{
    private const short MinHoursValue = 0;
    private const short MaxHoursValue = 24;

    public Hours(short value)
    {
        if (value < MinHoursValue || value > MaxHoursValue)
        {
            throw new ValidationException($"Invalid hours value. The hours must be in range between {MinHoursValue} and {MaxHoursValue}.");
        }

        Value = value;
    }

    public short Value { get; }

    public static implicit operator Hours(short hours) =>
        new(hours);

    public static implicit operator short(Hours hours) =>
        hours.Value;
}
