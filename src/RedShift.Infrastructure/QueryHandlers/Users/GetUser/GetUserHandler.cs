using MediatR;
using RedShift.Infrastructure.EF.DataLoaders;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.QueryHandlers.Users.GetUser;

internal sealed class GetUserHandler : IRequestHandler<GetUser, UserReadModel>
{
    private readonly UserByIdDataLoader _userDataLoader;

    public GetUserHandler(UserByIdDataLoader userDataLoader)
    {
        _userDataLoader = userDataLoader;
    }

    public async Task<UserReadModel> Handle(GetUser request, CancellationToken cancellationToken)
    {
        return await _userDataLoader.LoadAsync(request.Id, cancellationToken);
    }
}