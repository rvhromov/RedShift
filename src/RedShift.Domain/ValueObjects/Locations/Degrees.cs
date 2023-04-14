using RedShift.Domain.Exceptions;

namespace RedShift.Domain.ValueObjects.Locations;

public sealed record Degrees
{
    private const short MinDegreesValue = -90;
    private const short MaxDegreesValue = 90;

    public Degrees(short value)
    {
        if (value < MinDegreesValue || value > MaxDegreesValue)
        {
            throw new ValidationException($"Invalid degrees value. The degrees must be in range between {MinDegreesValue} and {MaxDegreesValue}.");
        }

        Value = value;
    }

    public short Value { get; }

    public static implicit operator short(Degrees degrees) =>
        degrees.Value;

    public static implicit operator Degrees(short degrees) =>
        new(degrees);
}