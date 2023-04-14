using HotChocolate.Authorization;
using HotChocolate.Data;
using MediatR;
using RedShift.Application.Services;
using RedShift.Domain.Enums;
using RedShift.GraphQL.Attributes;
using RedShift.GraphQL.TypeConfigs.Users;
using RedShift.Infrastructure.EF.ReadModels;
using RedShift.Infrastructure.QueryHandlers.Users.GetUser;
using RedShift.Infrastructure.QueryHandlers.Users.GetUsers;

namespace RedShift.GraphQL.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class UserQueries
{
    [AuthorizeRoles(RoleType.Admin)]
    [UsePaging]
    [UseProjection]
    [UseFiltering(typeof(UserFilterInputType))]
    [UseSorting]
    public async Task<IQueryable<UserReadModel>> GetUsersAsync([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetUsers(), cancellationToken);
    }

    [AuthorizeRoles(RoleType.Admin)]
    public async Task<UserReadModel> GetUserAsync(GetUser input, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    [Authorize]
    public async Task<UserReadModel> GetMeAsync(
        [Service] IIdentityAccessor identity, 
        [Service] IMediator mediator, 
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetUser(identity.GetUserId()), cancellationToken);
    }
}