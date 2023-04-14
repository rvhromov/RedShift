using RedShift.Domain.Exceptions;
using RedShift.Domain.ValueObjects.Location;
using Shouldly;

namespace RedShift.UnitTests.Domain;

public sealed class LocationTests
{
    [Fact]
    public void OnCreation_NoExceptionThrown()
    {
        var location = Location.Create("6 45 8.917, -16 42 58.02, ");

        location.ShouldNotBeNull();
        location.Declination.ShouldNotBe(default);
        location.RightAscension.ShouldNotBe(default);
    }

    [Fact]
    public void OnConvertingToDmsCoordinates_CoordinatesConvertedToOriginalValues()
    {
        var originalRightAscension = "6 45 8.917";
        var originalDeclination = "-16 42 58.02";

        var location = Location.Create($"{originalRightAscension}, {originalDeclination}");
        var (rightAscension, declination) = location.ToDmsCoordinates();

        rightAscension.ShouldNotBeNull();
        declination.ShouldNotBeNull();
        rightAscension.ShouldBeEquivalentTo(originalRightAscension);
        declination.ShouldBeEquivalentTo(originalDeclination);
    }

    [Theory]
    [InlineData("-16 42")]
    [InlineData("-16 42 58.02 14")]
    public void OnCreation_Throws_WhenNumberOfSubdivisionsOfDeclinationDoesntEqualThree(string declination)
    {
        var exception = Record.Exception(() => Location.Create($"6 45 8.917, {declination}"));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ValidationException>();
        exception.Message.ShouldBe("Invalid number of subdivisions of declination coordinates. Degrees, minutes and seconds are required.");
    }

    [Theory]
    [InlineData("45 8.917")]
    [InlineData("6 45 8 917")]
    public void OnCreation_Throws_WhenNumberOfSubdivisionsOfRightAscensionDoesntEqualThree(string rightAscension)
    {
        var exception = Record.Exception(() => Location.Create($"{rightAscension}, -16 42 58.02"));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ValidationException>();
        exception.Message.ShouldBe("Invalid number of subdivisions of right ascension coordinates. Hours, minutes and seconds are required.");
    }
}
