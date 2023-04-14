using Microsoft.EntityFrameworkCore;
using RedShift.Domain.Entities;
using RedShift.Domain.Repositories;
using RedShift.Infrastructure.EF.Contexts;

namespace RedShift.Infrastructure.EF.Repositories;

internal sealed class UserRepository : IUserRepository, IAsyncDisposable
{
    private readonly RedShiftWriteDbContext _context;

    public UserRepository(RedShiftWriteDbContext context) => 
        _context = context;

    public Task<User> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        var result = await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var result = _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}
