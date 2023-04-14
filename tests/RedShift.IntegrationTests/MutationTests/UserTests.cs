using FluentAssertions;
using NSubstitute;
using RedShift.Application.CommandHandlers.Registration.SignUp;
using RedShift.Application.CommandHandlers.Users.DeleteIdentity;
using RedShift.Application.CommandHandlers.Users.DeleteUser;
using RedShift.Application.CommandHandlers.Users.UpdateIdentity;
using RedShift.Application.CommandHandlers.Users.UpdateUser;
using RedShift.Application.Services;
using RedShift.Domain.Exceptions;
using RedShift.Infrastructure.EF.ReadModels;
using RedShift.IntegrationTests.Base;

namespace RedShift.IntegrationTests.MutationTests;

public sealed class UserTests : TestBase
{
    private static readonly UpdateUser UpdateUserCommand = new(Guid.Empty, "Dee Updated", "jon@email.com");
    private static readonly UpdateIdentity UpdateIdentityCommand = new("Dee Updated", "jon@email.com");
    private static readonly SignUp SignUpCommand = new("Jon Dee", "dee@email.com", "qwerty", "qwerty");

    [Fact]
    public async Task UpdateUser_WhenUserDoesntExistInSystem_ExceptionThrown()
    {
        await FluentActions
            .Invoking(() => SendAsync(UpdateUserCommand))
            .Should()
            .ThrowAsync<NotFoundException>("User not found.");
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        var user = await SendAsync(SignUpCommand);
        await SendAsync(UpdateUserCommand with { UserId = user.Id });

        var updatedUser = await GetEntityAsync<UserReadModel>(user.Id);

        updatedUser.Should().NotBeNull();
        updatedUser.Name.Should().Be(UpdateUserCommand.Name);
        updatedUser.Email.Should().Be(UpdateUserCommand.Email);
    }

    [Fact]
    public async Task UpdateIdentity_WhenUserDoesntExistInSystem_ExceptionThrown()
    {
        GetMockedService<IIdentityAccessor>()
            .GetUserId()
            .Returns(Guid.Empty);

        await FluentActions
            .Invoking(() => SendAsync(UpdateIdentityCommand))
            .Should()
            .ThrowAsync<NotFoundException>("User not found.");
    }

    [Fact]
    public async Task UpdateIdentity_ShouldUpdateIdentity()
    {
        var user = await SendAsync(SignUpCommand);

        GetMockedService<IIdentityAccessor>()
            .GetUserId()
            .Returns(user.Id);

        await SendAsync(UpdateIdentityCommand);

        var updatedUser = await GetEntityAsync<UserReadModel>(user.Id);

        updatedUser.Should().NotBeNull();
        updatedUser.Name.Should().Be(UpdateUserCommand.Name);
        updatedUser.Email.Should().Be(UpdateUserCommand.Email);
    }

    [Fact]
    public async Task DeleteUser_WhenUserDoesntExistInSystem_ExceptionThrown()
    {
        await FluentActions
            .Invoking(() => SendAsync(new DeleteUser(Guid.Empty)))
            .Should()
            .ThrowAsync<NotFoundException>("User not found.");
    }

    [Fact]
    public async Task DeleteUser_ShouldDeleteUser()
    {
        var user = await SendAsync(SignUpCommand);

        await SendAsync(new DeleteUser(user.Id));

        var deletedUser = await GetEntityAsync<UserReadModel>(user.Id);

        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task DeleteIdentity_WhenUserDoesntExistInSystem_ExceptionThrown()
    {
        GetMockedService<IIdentityAccessor>()
            .GetUserId()
            .Returns(Guid.Empty);

        await FluentActions
            .Invoking(() => SendAsync(new DeleteIdentity()))
            .Should()
            .ThrowAsync<NotFoundException>("User not found.");
    }

    [Fact]
    public async Task DeleteIdentity_ShouldDeleteIdentity()
    {
        var user = await SendAsync(SignUpCommand);

        GetMockedService<IIdentityAccessor>()
            .GetUserId()
            .Returns(user.Id);

        await SendAsync(new DeleteIdentity());

        var deletedIdentity = await GetEntityAsync<UserReadModel>(user.Id);

        deletedIdentity.Should().BeNull();
    }
}
