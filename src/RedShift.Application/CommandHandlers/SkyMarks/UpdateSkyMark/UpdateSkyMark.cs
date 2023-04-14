using MediatR;
using RedShift.Domain.Entities;

namespace RedShift.Application.CommandHandlers.SkyMarks.UpdateSkyMark;

public sealed record UpdateSkyMark(Guid SkyMarkId, string Title, string Description, string Coordinates) : IRequest<SkyMark>;
