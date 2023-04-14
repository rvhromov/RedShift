using RedShift.Domain.Exceptions;

namespace RedShift.Domain.ValueObjects.Locations;

public sealed record Seconds
{
    private const double MinSecondsValue = 0.0;
    private const double MaxSecondsValue = 60.0;

    public Seconds(double value)
    {
        if (value < MinSecondsValue || value > MaxSecondsValue)
        {
            throw new ValidationException($"Invalid seconds value. The seconds must be in range between {MinSecondsValue} and {MaxSecondsValue}.");
        }

        Value = value;
    }

    public double Value { get; }

    public static implicit operator Seconds(double seconds) =>
        new(seconds);

    public static implicit operator double(Seconds seconds) =>
        seconds.Value;
}
