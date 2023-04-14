using RedShift.Domain.Entities;
using Shouldly;

namespace RedShift.UnitTests.Domain;

public sealed class UserTests
{
    [Fact]
    public void Create_NoExceptionThrown()
    {
        var user = User.Create("James", "jwebb@email.com", "qwerty", "123");

        user.ShouldNotBeNull();
        user.Id.ShouldNotBe(default);
        user.CreatedAt.ShouldNotBe(default);
    }
}

