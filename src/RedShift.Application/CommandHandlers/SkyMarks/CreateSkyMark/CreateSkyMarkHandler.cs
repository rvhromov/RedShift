using MediatR;
using RedShift.Application.Services;
using RedShift.Domain.Entities;
using RedShift.Domain.Exceptions;
using RedShift.Domain.Repositories;
using RedShift.Domain.ValueObjects.Location;

namespace RedShift.Application.CommandHandlers.SkyMarks.CreateSkyMark;

internal sealed class CreateSkyMarkHandler : IRequestHandler<CreateSkyMark, SkyMark>
{
    private readonly ISkyMarkRepository _skyMarkRepository;
    private readonly IUserRepository _userRepository;
    private readonly IIdentityAccessor _identity;

    public CreateSkyMarkHandler(
        ISkyMarkRepository skyMarkRepository,
        IUserRepository userRepository,
        IIdentityAccessor identity)
    {
        _skyMarkRepository = skyMarkRepository;
        _userRepository = userRepository;
        _identity = identity;
    }

    public async Task<SkyMark> Handle(CreateSkyMark request, CancellationToken cancellationToken)
    {
        var userId = _identity.GetUserId();
        var (title, description, image, coordinates) = request;

        var user = await _userRepository.GetAsync(userId, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        var location = Location.Create(coordinates);
        var skyMark = SkyMark.Create(title, description, image, location, user);

        await _skyMarkRepository.AddAsync(skyMark, cancellationToken);

        return skyMark;
    }
}
