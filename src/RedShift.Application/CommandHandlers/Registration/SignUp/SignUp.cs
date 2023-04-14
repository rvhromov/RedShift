using MediatR;
using RedShift.Domain.Entities;

namespace RedShift.Application.CommandHandlers.Registration.SignUp;

public sealed record SignUp(string Name, string Email, string Password, string ConfirmPassword) : IRequest<User>;