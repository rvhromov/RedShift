using MediatR;
using RedShift.Application.Services;
using RedShift.Domain.Enums;
using RedShift.Domain.Exceptions;
using RedShift.Domain.Repositories;

namespace RedShift.Application.CommandHandlers.SkyMarks.DeleteSkyMark;

internal sealed class DeleteSkyMarkHandler : IRequestHandler<DeleteSkyMark, DeleteSkyMarkPayload>
{
    private readonly IIdentityAccessor _identity;
    private readonly ISkyMarkRepository _repository;

    public DeleteSkyMarkHandler(IIdentityAccessor identity, ISkyMarkRepository repository)
    {
        _identity = identity;
        _repository = repository;
    }

    public async Task<DeleteSkyMarkPayload> Handle(DeleteSkyMark request, CancellationToken cancellationToken)
    {
        var user = _identity.GetUserIdAndRole();

        var skyMark = user.role is RoleType.Admin
            ? await _repository.GetAsync(request.Id, cancellationToken)
            : await _repository.GetAsync(request.Id, user.id, cancellationToken);

        if (skyMark is null)
        {
            throw new NotFoundException("Sky mark not found.");
        }

        await _repository.DeleteAsync(skyMark, cancellationToken);

        return new DeleteSkyMarkPayload(request.Id);
    }
}
