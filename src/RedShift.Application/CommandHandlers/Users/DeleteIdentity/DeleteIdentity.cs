using MediatR;

namespace RedShift.Application.CommandHandlers.Users.DeleteIdentity;

public sealed record DeleteIdentity() : IRequest<DeleteIdentityPayload>;
