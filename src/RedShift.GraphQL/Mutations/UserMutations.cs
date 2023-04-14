using HotChocolate.Authorization;
using MediatR;
using RedShift.Application.CommandHandlers.Users.DeleteIdentity;
using RedShift.Application.CommandHandlers.Users.DeleteUser;
using RedShift.Application.CommandHandlers.Users.UpdateIdentity;
using RedShift.Application.CommandHandlers.Users.UpdateUser;
using RedShift.Domain.Entities;
using RedShift.Domain.Enums;
using RedShift.GraphQL.Attributes;

namespace RedShift.GraphQL.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class UserMutations
{
    [AuthorizeRoles(RoleType.Admin)]
    public async Task<User> UpdateUserAsync(UpdateUser input, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    [Authorize]
    public async Task<User> UpdateIdentityAsync(UpdateIdentity input, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    [AuthorizeRoles(RoleType.Admin)]
    public async Task<DeleteUserPayload> DeleteUserAsync(DeleteUser input, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    [AuthorizeRoles(RoleType.User)]
    public async Task<DeleteIdentityPayload> DeleteIdentityAsync([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new DeleteIdentity(), cancellationToken);
    }
}
