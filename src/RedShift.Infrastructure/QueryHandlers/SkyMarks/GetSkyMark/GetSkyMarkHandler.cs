using MediatR;
using RedShift.Infrastructure.EF.DataLoaders;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.QueryHandlers.SkyMarks.GetSjyMark;

internal sealed class GetSkyMarkHandler : IRequestHandler<GetSkyMark, SkyMarkReadModel>
{
    private readonly SkyMarkByIdDataLoader _dataLoader;

    public GetSkyMarkHandler(SkyMarkByIdDataLoader dataLoader)
    {
        _dataLoader = dataLoader;
    }

    public async Task<SkyMarkReadModel> Handle(GetSkyMark request, CancellationToken cancellationToken)
    {
        return await _dataLoader.LoadAsync(request.SkyMarkId, cancellationToken);
    }
}
