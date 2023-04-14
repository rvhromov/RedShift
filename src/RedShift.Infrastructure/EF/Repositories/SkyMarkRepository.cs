using Microsoft.EntityFrameworkCore;
using RedShift.Domain.Entities;
using RedShift.Domain.Repositories;
using RedShift.Infrastructure.EF.Contexts;

namespace RedShift.Infrastructure.EF.Repositories;

internal sealed class SkyMarkRepository : ISkyMarkRepository, IAsyncDisposable
{
    private readonly RedShiftWriteDbContext _context;

    public SkyMarkRepository(RedShiftWriteDbContext context) =>
        _context = context;

    public Task<SkyMark> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.SkyMarks.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<SkyMark> GetAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.SkyMarks.SingleOrDefaultAsync(s => s.Id == id && s.User.Id == userId, cancellationToken);
    }

    public async Task<SkyMark> AddAsync(SkyMark skyMark, CancellationToken cancellationToken)
    {
        var result = await _context.AddAsync(skyMark, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task<SkyMark> UpdateAsync(SkyMark skyMark, CancellationToken cancellationToken)
    {
        var result = _context.SkyMarks.Update(skyMark);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task DeleteAsync(SkyMark skyMark, CancellationToken cancellationToken)
    {
        _context.SkyMarks.Remove(skyMark);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}
