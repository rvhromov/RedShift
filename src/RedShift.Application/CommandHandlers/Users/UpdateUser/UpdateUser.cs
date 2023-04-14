using MediatR;
using RedShift.Domain.Entities;

namespace RedShift.Application.CommandHandlers.Users.UpdateUser;

public sealed record UpdateUser(Guid UserId, string Name, string Email) : IRequest<User>;