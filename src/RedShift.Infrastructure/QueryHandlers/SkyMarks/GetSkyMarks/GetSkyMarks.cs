using MediatR;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.QueryHandlers.SkyMarks.GetSkyMarks;

public sealed record GetSkyMarks() : IRequest<IQueryable<SkyMarkReadModel>>;
