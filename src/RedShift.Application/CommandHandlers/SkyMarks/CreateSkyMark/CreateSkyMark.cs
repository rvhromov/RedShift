using MediatR;
using RedShift.Domain.Entities;

namespace RedShift.Application.CommandHandlers.SkyMarks.CreateSkyMark;

public sealed record CreateSkyMark(string Title, string Description, string Image, string Coordinates) : IRequest<SkyMark>;