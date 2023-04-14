using RedShift.Domain.Entities;

namespace RedShift.Domain.Repositories;

public interface IUserRepository
{
    public Task<User> GetAsync(Guid id, CancellationToken cancellationToken);
    public Task<User> AddAsync(User user, CancellationToken cancellationToken);
    public Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
    public Task DeleteAsync(User user, CancellationToken cancellationToken);
}
