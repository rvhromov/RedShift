using FluentAssertions;
using NSubstitute;
using RedShift.Application.CommandHandlers.Registration.SignUp;
using RedShift.Application.CommandHandlers.SkyMarks.CreateSkyMark;
using RedShift.Application.CommandHandlers.SkyMarks.DeleteSkyMark;
using RedShift.Application.CommandHandlers.SkyMarks.UpdateSkyMark;
using RedShift.Application.Services;
using RedShift.Domain.Entities;
using RedShift.Domain.Exceptions;
using RedShift.Domain.ValueObjects.Location;
using RedShift.Infrastructure.EF.ReadModels;
using RedShift.IntegrationTests.Base;

namespace RedShift.IntegrationTests.MutationTests;

public sealed class SkyMarkTests : TestBase
{
    private static readonly CreateSkyMark CreateSkyMarkCommand = new("Sirius", default, default, "6 45 8.917, -16 42 58.02");
    private static readonly SignUp SignUpCommand = new("Jon Dee", "dee@email.com", "qwerty", "qwerty");
    private static readonly UpdateSkyMark UpdateSkyMarkCommand = new UpdateSkyMark(Guid.Empty, "Polaris", default, "2 31 49.09, 89 15 50.8");

    [Fact]
    public async Task CreateSkyMark_WhenCurrentUserDoesntExistInSystem_ExceptionThrown()
    {
        GetMockedService<IIdentityAccessor>()
            .GetUserId()
            .Returns(Guid.Empty);

        await FluentActions
            .Invoking(() => SendAsync(CreateSkyMarkCommand))
            .Should()
            .ThrowAsync<NotFoundException>("User not found.");
    }

    [Fact]
    public async Task CreateSkyMark_ShouldCreateSkyMark()
    {
        var user = await SendAsync(SignUpCommand);

        GetMockedService<IIdentityAccessor>()
            .GetUserId()
            .Returns(user.Id);

        var newSkyMark = await SendAsync(CreateSkyMarkCommand);

        var skyMark = await GetEntityAsync<SkyMarkReadModel>(newSkyMark.Id);

        skyMark.Should().NotBeNull();
        skyMark.Title.Should().Be(CreateSkyMarkCommand.Title);

        var (rightAscension, declination) = skyMark.Location.ToDmsCoordinates();

        rightAscension.Should().NotBeNullOrWhiteSpace().And.Be("6 45 8.917");
        declination.Should().NotBeNullOrWhiteSpace().And.Be("-16 42 58.02");
    }

    [Fact]
    public async Task UpdateSkyMark_WhenUserUpdatesSomeoneElsesSkyMark_ExceptionThrown()
    {
        var user1 = await SendAsync(SignUpCommand);
        var user2 = await SendAsync(SignUpCommand with { Email = "jon@email.com" });

        GetMockedService<IIdentityAccessor>()
            .GetUserIdAndRole()
            .Returns((user1.Id, user1.Role));

        var location = Location.Create("6 45 8.917, -16 42 58.02");
        var skyMark = SkyMark.Create("Sirius", default, default, location, user2);

        var user2SkyMark = await AddEntityAsync(skyMark);

        await FluentActions
            .Invoking(() => SendAsync(UpdateSkyMarkCommand with { SkyMarkId = user2SkyMark .Id }))
            .Should()
            .ThrowAsync<NotFoundException>("Sky mark not found.");
    }

    [Fact]
    public async Task UpdateSkyMark_ShouldUpdateSkyMark()
    {
        var user = await SendAsync(SignUpCommand);

        GetMockedService<IIdentityAccessor>()
            .GetUserIdAndRole()
            .Returns((user.Id, user.Role));

        GetMockedService<IIdentityAccessor>()
            .GetUserId()
            .Returns(user.Id);

        var newSkyMark = await SendAsync(CreateSkyMarkCommand);

        await SendAsync(UpdateSkyMarkCommand with { SkyMarkId = newSkyMark.Id });

        var updatedSkyMark = await GetEntityAsync<SkyMarkReadModel>(newSkyMark.Id);

        updatedSkyMark.Should().NotBeNull();
        updatedSkyMark.Title.Should().Be(UpdateSkyMarkCommand.Title);

        var (rightAscension, declination) = updatedSkyMark.Location.ToDmsCoordinates();

        rightAscension.Should().NotBeNullOrWhiteSpace().And.Be("2 31 49.09");
        declination.Should().NotBeNullOrWhiteSpace().And.Be("89 15 50.8");
    }

    [Fact]
    public async Task DeleteSkyMark_WhenCurrentUserDoesntExistInSystem_ExceptionThrown()
    {
        await FluentActions
            .Invoking(() => SendAsync(new DeleteSkyMark(Guid.Empty)))
            .Should()
            .ThrowAsync<NotFoundException>("Sky mark not found.");
    }

    [Fact]
    public async Task DeleteSkyMark_ShouldDeleteSkyMark()
    {
        var user = await SendAsync(SignUpCommand);

        GetMockedService<IIdentityAccessor>()
            .GetUserIdAndRole()
            .Returns((user.Id, user.Role));

        GetMockedService<IIdentityAccessor>()
            .GetUserId()
            .Returns(user.Id);

        var newSkyMark = await SendAsync(CreateSkyMarkCommand);

        await SendAsync(new DeleteSkyMark(newSkyMark.Id));

        var deletedSkyLine = await GetEntityAsync<SkyMarkReadModel>(newSkyMark.Id);

        deletedSkyLine.Should().BeNull();
    }
}
