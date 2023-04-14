using MediatR;
using RedShift.Domain.Entities;
using RedShift.Domain.Exceptions;
using RedShift.Domain.Repositories;

namespace RedShift.Application.CommandHandlers.Users.UpdateUser;

internal sealed class UpdateUserHandler : IRequestHandler<UpdateUser, User>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(UpdateUser request, CancellationToken cancellationToken)
    {
        var (userId, name, email) = request;

        var user = await _userRepository.GetAsync(userId, cancellationToken) 
            ?? throw new NotFoundException("User not found.");

        user.Update(name, email);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return user;
    }
}
