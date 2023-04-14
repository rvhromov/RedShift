using MediatR;
using RedShift.Domain.Enums;
using RedShift.Domain.Exceptions;
using RedShift.Domain.Repositories;

namespace RedShift.Application.CommandHandlers.Users.DeleteUser;

internal sealed class DeleteUserHandler : IRequestHandler<DeleteUser, DeleteUserPayload>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<DeleteUserPayload> Handle(DeleteUser request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException("User nor found.");

        if (user.Role is RoleType.Admin)
        {
            throw new ValidationException("Admin user cannot be deleted.");
        }

        await _userRepository.DeleteAsync(user, cancellationToken);

        return new DeleteUserPayload(request.Id);
    }
}
