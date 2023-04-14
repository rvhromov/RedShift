using MediatR;

namespace RedShift.Application.CommandHandlers.Users.DeleteUser;

public sealed record DeleteUser(Guid Id) : IRequest<DeleteUserPayload>;