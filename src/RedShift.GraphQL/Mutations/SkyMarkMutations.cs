using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using MediatR;
using RedShift.Application.CommandHandlers.SkyMarks.CreateSkyMark;
using RedShift.Application.CommandHandlers.SkyMarks.DeleteSkyMark;
using RedShift.Application.CommandHandlers.SkyMarks.UpdateSkyMark;
using RedShift.Domain.Entities;
using RedShift.GraphQL.Subscriptions;

namespace RedShift.GraphQL.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class SkyMarkMutations
{
    [Authorize]
    public async Task<SkyMark> AddSkyMarkAsync(
        CreateSkyMark input, 
        [Service] IMediator mediator,
        [Service] ITopicEventSender sender,
        CancellationToken cancellationToken)
    {
        var skyMark = await mediator.Send(input, cancellationToken);

        await sender.SendAsync(nameof(SkyMarkSubscriptions.SkyMarkAdded), skyMark.Id, cancellationToken);

        return skyMark;
    }

    [Authorize]
    public async Task<SkyMark> UpdateSkyMarkAsync(UpdateSkyMark input, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    [Authorize]
    public async Task<DeleteSkyMarkPayload> DeleteSkyMarkAsync(
        DeleteSkyMark input, 
        [Service] IMediator mediator, 
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }
}