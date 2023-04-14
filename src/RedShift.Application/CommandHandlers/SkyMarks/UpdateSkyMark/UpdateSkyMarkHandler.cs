using MediatR;
using RedShift.Application.Services;
using RedShift.Domain.Entities;
using RedShift.Domain.Enums;
using RedShift.Domain.Exceptions;
using RedShift.Domain.Repositories;
using RedShift.Domain.ValueObjects.Location;

namespace RedShift.Application.CommandHandlers.SkyMarks.UpdateSkyMark;

internal sealed class UpdateSkyMarkHandler : IRequestHandler<UpdateSkyMark, SkyMark>
{
    private readonly IIdentityAccessor _identity;
    private readonly ISkyMarkRepository _repository;

    public UpdateSkyMarkHandler(IIdentityAccessor identity, ISkyMarkRepository repository)
    {
        _identity = identity;
        _repository = repository;
    }

    public async Task<SkyMark> Handle(UpdateSkyMark request, CancellationToken cancellationToken)
    {
        var user = _identity.GetUserIdAndRole();
        var (id, title, description, coordinates) = request;

        var skyMark = user.role is RoleType.Admin 
            ? await _repository.GetAsync(id, cancellationToken) 
            : await _repository.GetAsync(id, user.id, cancellationToken);

        if (skyMark is null)
        {
            throw new NotFoundException("Sky mark not found.");
        }

        var location = Location.Create(coordinates);
        skyMark.Update(title, description, location);

        await _repository.UpdateAsync(skyMark, cancellationToken);

        return skyMark;
    }
}
