using MediatR;
using RedShift.Application.Services;
using RedShift.Domain.Entities;
using RedShift.Domain.Exceptions;
using RedShift.Domain.Repositories;

namespace RedShift.Application.CommandHandlers.Users.UpdateIdentity;

internal sealed class UpdateIdentityHandler : IRequestHandler<UpdateIdentity, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityAccessor _identity;

    public UpdateIdentityHandler(IUserRepository userRepository, IIdentityAccessor identity)
    {
        _userRepository = userRepository;
        _identity = identity;
    }

    public async Task<User> Handle(UpdateIdentity request, CancellationToken cancellationToken)
    {
        var id = _identity.GetUserId();
        var (name, email) = request;

        var user = await _userRepository.GetAsync(id, cancellationToken)
            ?? throw new NotFoundException("User not found");

        user.Update(name, email);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return user;
    }
}
