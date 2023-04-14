using MediatR;
using RedShift.Application.Services;
using RedShift.Domain.Exceptions;

namespace RedShift.Application.CommandHandlers.Registration.SignIn;

internal sealed class SignInHandler : IRequestHandler<SignIn, SignInPayload>
{
    private readonly IUserReadService _userReadService;
    private readonly ISecurityService _securityService;
    private readonly ITokenService _tokenService;

    public SignInHandler(IUserReadService userReadService, ISecurityService securityService, ITokenService tokenService)
    {
        _userReadService = userReadService;
        _securityService = securityService;
        _tokenService = tokenService;
    }

    public async Task<SignInPayload> Handle(SignIn request, CancellationToken cancellationToken)
    {
        var user = await _userReadService.GetUserByEmailAsync(request.Email)
            ?? throw new NotFoundException($"User with email {request.Email} not found.");

        var passwordsMatch = _securityService.VerifyPassword(request.Password, user.Password, user.Salt);

        if (!passwordsMatch)
        {
            throw new ValidationException("Invalid password.");
        }

        var token = _tokenService.CreateAccessToken(user.Id, user.Role);

        return new SignInPayload
        {
            AccessToken = token
        };
    }
}
