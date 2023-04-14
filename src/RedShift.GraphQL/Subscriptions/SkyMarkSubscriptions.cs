using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using MediatR;
using RedShift.Infrastructure.EF.ReadModels;
using RedShift.Infrastructure.QueryHandlers.SkyMarks.GetSjyMark;

namespace RedShift.GraphQL.Subscriptions;

[ExtendObjectType(OperationTypeNames.Subscription)]
public sealed class SkyMarkSubscriptions
{
    [Subscribe(With = nameof(SubscribeToSkyMarks))]
    public async Task<SkyMarkReadModel> SkyMarkAdded(
        [EventMessage] Guid skyMarkId, 
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetSkyMark(skyMarkId), cancellationToken);
    }

    public async ValueTask<ISourceStream<Guid>> SubscribeToSkyMarks(
        [Service] ITopicEventReceiver receiver, 
        CancellationToken cancellationToken)
    {
        return await receiver.SubscribeAsync<Guid>(nameof(SkyMarkSubscriptions.SkyMarkAdded), cancellationToken);
    }  
}
