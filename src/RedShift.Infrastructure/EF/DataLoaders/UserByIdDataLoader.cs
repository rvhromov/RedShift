using GreenDonut;
using Microsoft.EntityFrameworkCore;
using RedShift.Infrastructure.EF.Contexts;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.EF.DataLoaders;

internal sealed class UserByIdDataLoader : BatchDataLoader<Guid, UserReadModel>
{
    private readonly IDbContextFactory<RedShiftReadDbContext> _dbContextFactory;

    public UserByIdDataLoader(
        IBatchScheduler batchScheduler, 
        DataLoaderOptions options = null, 
        IDbContextFactory<RedShiftReadDbContext> dbContextFactory = null) 
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory;
    }

    protected override async Task<IReadOnlyDictionary<Guid, UserReadModel>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Users
            .Include(u => u.SkyMarks)
            .AsNoTracking()
            .Where(u => keys.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, cancellationToken);
    }
}
