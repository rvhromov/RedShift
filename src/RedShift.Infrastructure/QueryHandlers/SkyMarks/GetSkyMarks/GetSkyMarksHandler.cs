using MediatR;
using Microsoft.EntityFrameworkCore;
using RedShift.Application.Services;
using RedShift.Domain.Enums;
using RedShift.Infrastructure.EF.Contexts;
using RedShift.Infrastructure.EF.ReadModels;
using System.Linq.Expressions;

namespace RedShift.Infrastructure.QueryHandlers.SkyMarks.GetSkyMarks;

internal sealed class GetSkyMarksHandler : IRequestHandler<GetSkyMarks, IQueryable<SkyMarkReadModel>>, IAsyncDisposable
{
    private readonly RedShiftReadDbContext _context;
    private readonly IIdentityAccessor _identity;

    public GetSkyMarksHandler(IDbContextFactory<RedShiftReadDbContext> contextFactory, IIdentityAccessor identity)
    {
        _context = contextFactory.CreateDbContext();
        _identity = identity;
    }

    public async Task<IQueryable<SkyMarkReadModel>> Handle(GetSkyMarks request, CancellationToken cancellationToken)
    {
        var user = _identity.GetUserIdAndRole();

        Expression<Func<SkyMarkReadModel, bool>> predicate = user.role is RoleType.User
            ? s => s.User.Id == user.id
            : s => true;

        return _context.SkyMarks
            .Include(s => s.User)
            .AsNoTracking()
            .Where(predicate);
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}