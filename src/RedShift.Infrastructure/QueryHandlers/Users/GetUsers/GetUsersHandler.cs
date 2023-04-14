using MediatR;
using Microsoft.EntityFrameworkCore;
using RedShift.Infrastructure.EF.Contexts;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.QueryHandlers.Users.GetUsers;

internal sealed class GetUsersHandler : IRequestHandler<GetUsers, IQueryable<UserReadModel>>, IAsyncDisposable
{
    private readonly RedShiftReadDbContext _context;

    public GetUsersHandler(IDbContextFactory<RedShiftReadDbContext> contextFactory)
    {
        _context = contextFactory.CreateDbContext();
    }

    public async Task<IQueryable<UserReadModel>> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        return _context.Users
            .Include(u => u.SkyMarks)
            .AsNoTracking();
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}
