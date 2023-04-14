using RedShift.Application.CommandHandlers.Registration.SignUp;
using FluentAssertions;
using RedShift.Domain.Exceptions;
using RedShift.IntegrationTests.Base;
using RedShift.Application.CommandHandlers.Registration.SignIn;

namespace RedShift.IntegrationTests.MutationTests;

public sealed class RegistrationTests : TestBase
{
    private static readonly SignUp SignUpCommand = new("Jon Dee", "dee@email.com", "qwerty", "qwerty");
    private static readonly SignIn SignInCommand = new("dee@email.com", "qwerty");

    [Fact]
    public async Task SignUp_WhenUserWithTheSameEmailExists_ExceptionThrown()
    {
        await SendAsync(SignUpCommand);

        await FluentActions
            .Invoking(() => SendAsync(SignUpCommand))
            .Should()
            .ThrowAsync<ValidationException>("User with the same email address already exists.");
    }

    [Fact]
    public async Task SignUp_WhenPasswordDoesntMatchConfirmPassword_ExceptionThrown()
    {
        await FluentActions
            .Invoking(() => SendAsync(SignUpCommand with { ConfirmPassword = "ytrewq" }))
            .Should()
            .ThrowAsync<ValidationException>("Confirmation password doesn't match original.");
    }

    [Fact]
    public async Task SignUp_ShouldSignUpNewUser()
    {
        var newUser = await SendAsync(SignUpCommand);

        newUser.Should().NotBeNull();
        newUser.Email.Value.Should().Be(SignUpCommand.Email);
        newUser.Password.Should().NotBeNullOrWhiteSpace();
        newUser.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task SignIn_WhenUserDoesntExistInSystem_ExceptionThrown()
    {
        await FluentActions
            .Invoking(() => SendAsync(SignInCommand))
            .Should()
            .ThrowAsync<NotFoundException>($"User with email {SignInCommand.Email} not found.");
    }

    [Fact]
    public async Task SignIn_WhenInvalidPasswordPassed_ExceptionThrown()
    {
        await SendAsync(SignUpCommand);

        await FluentActions
            .Invoking(() => SendAsync(SignInCommand with { Password = "ytrewq" }))
            .Should()
            .ThrowAsync<ValidationException>("Invalid password.");
    }

    [Fact]
    public async Task SignIn_ShouldSignInUser()
    {
        await SendAsync(SignUpCommand);

        var result = await SendAsync(SignInCommand);

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
    }
}
