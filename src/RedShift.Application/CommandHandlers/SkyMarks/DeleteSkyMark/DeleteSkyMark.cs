using MediatR;

namespace RedShift.Application.CommandHandlers.SkyMarks.DeleteSkyMark;

public sealed record DeleteSkyMark(Guid Id) : IRequest<DeleteSkyMarkPayload>;
