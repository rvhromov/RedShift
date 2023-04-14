using RedShift.Domain.Exceptions;
using RedShift.Domain.ValueObjects.Locations;
using System.Globalization;

namespace RedShift.Domain.ValueObjects.Location;

public sealed record Location
{
    private const int NumberOfDecSubdivisions = 3;
    private const int NumberOfRaSubdivisions = 3;

    private static readonly NumberFormatInfo Format = new()
    {
        NegativeSign = "-",
        NumberDecimalSeparator = "."
    };

    private Location()
    {
    }

    public double RightAscension { get; private set; }
    public double Declination { get; private set; }

    public static Location Create(string location)
    {
        var coordinates = location.Split(',');

        var parsedRightAscention = ParseRightAscension(coordinates[0].Trim());
        var parsedDeclination = ParseDeclination(coordinates[1].Trim());

        return new()
        {
            RightAscension = ConvertRightAscensionToDecimal(parsedRightAscention),
            Declination = ConvertDeclinationToDecimal(parsedDeclination)
        };
    }

    // DMS stands for Degrees, Minutes, Seconds
    public (string rightAscension, string declination) ToDmsCoordinates()
    {
        var rightAscension = $"{ExtractHours(RightAscension)} {ExtractMinutes(RightAscension)} {ExtractSeconds(RightAscension).ToString(Format)}";
        var declination = $"{ExtractDegrees(Declination)} {ExtractMinutes(Declination)} {ExtractSeconds(Declination).ToString(Format)}";

        return (rightAscension, declination);
    }

    private static (Degrees degrees, Minutes minutes, Seconds seconds) ParseDeclination(string declination)
    {
        if (string.IsNullOrWhiteSpace(declination))
        {
            throw new ValidationException("Declination cannot be null or empty.");
        }

        var coordinates = declination.Split(' ');

        if (coordinates.Length != NumberOfDecSubdivisions)
        {
            throw new ValidationException("Invalid number of subdivisions of declination coordinates. Degrees, minutes and seconds are required.");
        }

        var degreesParsed = short.TryParse(coordinates[0], NumberStyles.AllowLeadingSign, Format, out var degrees);
        var minutesParsed = short.TryParse(coordinates[1], Format, out var minutes);
        var secondsParsed = double.TryParse(coordinates[2], NumberStyles.AllowDecimalPoint, Format, out var seconds);

        if (!degreesParsed || !minutesParsed || !secondsParsed)
        {
            throw new ValidationException("Declination string contains invalid characters.");
        }

        return (degrees, minutes, seconds);
    }

    private static (Hours hours, Minutes minutes, Seconds seconds) ParseRightAscension(string rightAscension)
    {
        if (string.IsNullOrWhiteSpace(rightAscension))
        {
            throw new ValidationException("Right ascension cannot be null or empty.");
        }

        var coordinates = rightAscension.Split(' ');

        if (coordinates.Length != NumberOfRaSubdivisions)
        {
            throw new ValidationException("Invalid number of subdivisions of right ascension coordinates. Hours, minutes and seconds are required.");
        }

        var hoursParsed = short.TryParse(coordinates[0], Format, out var hours);
        var minutesParsed = short.TryParse(coordinates[1], Format, out var minutes);
        var secondsParsed = double.TryParse(coordinates[2], NumberStyles.AllowDecimalPoint, Format, out var seconds);

        if (!hoursParsed || !minutesParsed || !secondsParsed)
        {
            throw new ValidationException("Right ascension string contains invalid characters.");
        }

        return (hours, minutes, seconds);
    }

    private static double ConvertDeclinationToDecimal((Degrees degrees, Minutes minutes, Seconds seconds) declination)
    {
        var (degrees, minutes, seconds) = declination;

        var multiplier = degrees >= 0 ? 1 : -1;

        var absoluteDegrees = Math.Abs(degrees);
        var result = absoluteDegrees + minutes / 60d + seconds / 3600d;

        return result * multiplier;
    }

    private static double ConvertRightAscensionToDecimal((Hours hours, Minutes minutes, Seconds seconds) rightAscention)
    {
        var (hours, minutes, seconds) = rightAscention;

        return hours + minutes / 60d + seconds / 3600d;
    }

    private static short ExtractDegrees(double value) =>
        (short)value;

    private static short ExtractHours(double value) =>
        (short)value;

    private static short ExtractMinutes(double value)
    {
        var absoluteValue = Math.Abs(value);
        return (short)((absoluteValue - ExtractDegrees(absoluteValue)) * 60);
    }

    private static double ExtractSeconds(double value)
    {
        var absoluteValue = Math.Abs(value);
        var minutes = (absoluteValue - ExtractDegrees(absoluteValue)) * 60;
        return Math.Round((minutes - ExtractMinutes(absoluteValue)) * 60, 3);
    }
}
