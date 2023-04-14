using MediatR;

namespace RedShift.Application.CommandHandlers.Registration.SignIn;

public sealed record SignIn(string Email, string Password) : IRequest<SignInPayload>;
