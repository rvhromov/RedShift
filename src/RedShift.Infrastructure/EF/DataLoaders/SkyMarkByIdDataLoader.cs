using GreenDonut;
using Microsoft.EntityFrameworkCore;
using RedShift.Application.Services;
using RedShift.Domain.Enums;
using RedShift.Infrastructure.EF.Contexts;
using RedShift.Infrastructure.EF.ReadModels;
using System.Linq.Expressions;

namespace RedShift.Infrastructure.EF.DataLoaders;

internal sealed class SkyMarkByIdDataLoader : BatchDataLoader<Guid, SkyMarkReadModel>
{
    private readonly IDbContextFactory<RedShiftReadDbContext> _dbContextFactory;
    private readonly IIdentityAccessor _identity;

    public SkyMarkByIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options = null,
        IDbContextFactory<RedShiftReadDbContext> dbContextFactory = null,
        IIdentityAccessor identity = null)
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory;
        _identity = identity;
    }

    protected override async Task<IReadOnlyDictionary<Guid, SkyMarkReadModel>> LoadBatchAsync(
        IReadOnlyList<Guid> keys, 
        CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var (userId, userRole) = _identity.GetUserIdAndRole();

        Expression<Func<SkyMarkReadModel, bool>> predicate = userRole is RoleType.Admin 
            ? u => keys.Contains(u.Id) 
            : u => u.User.Id == userId && keys.Contains(u.Id);

        return await context.SkyMarks
            .Include(u => u.User)
            .AsNoTracking()
            .Where(predicate)
            .ToDictionaryAsync(u => u.Id, cancellationToken);
    }
}
