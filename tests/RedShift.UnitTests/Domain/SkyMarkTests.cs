using RedShift.Domain.Entities;
using RedShift.Domain.ValueObjects.Location;
using Shouldly;

namespace RedShift.UnitTests.Domain;

public sealed class SkyMarkTests
{
    [Fact]
    public void Create_NoExceptionThrown()
    {
        var location = Location.Create("6 45 8.917, -16 42 58.02");
        var user = User.Create("James", "jwebb@email.com", "qwerty", "123");

        var skyMark = SkyMark.Create("Sirius", default, "azure.blob.net/qwerty123", location, user);

        skyMark.ShouldNotBeNull();
        skyMark.Id.ShouldNotBe(default);
        skyMark.CreatedAt.ShouldNotBe(default);
        skyMark.Location.Declination.ShouldNotBe(default);
        skyMark.Location.RightAscension.ShouldNotBe(default);
    }
}
