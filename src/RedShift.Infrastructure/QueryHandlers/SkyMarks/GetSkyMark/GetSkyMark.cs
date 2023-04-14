using MediatR;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.QueryHandlers.SkyMarks.GetSjyMark;

public sealed record GetSkyMark(Guid SkyMarkId) : IRequest<SkyMarkReadModel>;
