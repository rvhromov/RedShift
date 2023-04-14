using MediatR;
using RedShift.Application.Services;
using RedShift.Domain.Entities;
using RedShift.Domain.Exceptions;
using RedShift.Domain.Repositories;

namespace RedShift.Application.CommandHandlers.Registration.SignUp;

internal sealed class SignUpHandler : IRequestHandler<SignUp, User>
{
    private readonly IUserReadService _userReadService;
    private readonly ISecurityService _securityService;
    private readonly IUserRepository _userRepository;

    public SignUpHandler(IUserReadService userReadService, ISecurityService securityService, IUserRepository userRepository)
    {
        _userReadService = userReadService;
        _securityService = securityService;
        _userRepository = userRepository;
    }

    public async Task<User> Handle(SignUp request, CancellationToken cancellationToken)
    {
        var (name, email, password, confirmPassword) = request;

        if (await _userReadService.ExistsAsync(email))
        {
            throw new ValidationException("User with the same email address already exists.");
        }

        if (!_securityService.PasswordMatch(password, confirmPassword))
        {
            throw new ValidationException("Confirmation password doesn't match original.");
        }

        var hashedPassword = _securityService.HashPassword(password);

        var user = User.Create(name, email, hashedPassword.Password, hashedPassword.Salt);
        await _userRepository.AddAsync(user, cancellationToken);

        return user;
    }
}
