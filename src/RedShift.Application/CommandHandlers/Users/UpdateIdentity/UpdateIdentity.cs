using MediatR;
using RedShift.Domain.Entities;

namespace RedShift.Application.CommandHandlers.Users.UpdateIdentity;

public sealed record UpdateIdentity(string Name, string Email) : IRequest<User>;
