using MediatR;
using RedShift.Application.Services;
using RedShift.Domain.Exceptions;
using RedShift.Domain.Repositories;

namespace RedShift.Application.CommandHandlers.Users.DeleteIdentity;

internal sealed class DeleteIdentityHandler : IRequestHandler<DeleteIdentity, DeleteIdentityPayload>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityAccessor _identityAccessor;

    public DeleteIdentityHandler(IUserRepository userRepository, IIdentityAccessor identityAccessor)
    {
        _userRepository = userRepository;
        _identityAccessor = identityAccessor;
    }

    public async Task<DeleteIdentityPayload> Handle(DeleteIdentity request, CancellationToken cancellationToken)
    {
        var id = _identityAccessor.GetUserId();

        var user = await _userRepository.GetAsync(id, cancellationToken)
            ?? throw new NotFoundException("User nor found.");

        await _userRepository.DeleteAsync(user, cancellationToken);

        return new DeleteIdentityPayload(id);
    }
}
