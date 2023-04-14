using RedShift.Domain.Entities;

namespace RedShift.Domain.Repositories;

public interface ISkyMarkRepository
{
    Task<SkyMark> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<SkyMark> GetAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<SkyMark> AddAsync(SkyMark skyMark, CancellationToken cancellationToken);
    Task<SkyMark> UpdateAsync(SkyMark skyMark, CancellationToken cancellationToken);
    Task DeleteAsync(SkyMark skyMark, CancellationToken cancellationToken);
}
