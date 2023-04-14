using HotChocolate.Authorization;
using HotChocolate.Data;
using MediatR;
using RedShift.GraphQL.TypeConfigs.SkyMarks;
using RedShift.Infrastructure.EF.ReadModels;
using RedShift.Infrastructure.QueryHandlers.SkyMarks.GetSjyMark;
using RedShift.Infrastructure.QueryHandlers.SkyMarks.GetSkyMarks;

namespace RedShift.GraphQL.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class SkyMarkQueries
{
    [Authorize]
    [UsePaging]
    [UseProjection]
    [UseFiltering(typeof(SkyMarkFilterInputType))]
    [UseSorting]
    public async Task<IQueryable<SkyMarkReadModel>> GetSkyMarksAsync([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetSkyMarks(), cancellationToken);
    }

    [Authorize]
    public async Task<SkyMarkReadModel> GetSkyMarkAsync(GetSkyMark input, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }
}