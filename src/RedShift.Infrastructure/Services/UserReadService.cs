using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RedShift.Application.Dtos.Users;
using RedShift.Application.Services;
using RedShift.Infrastructure.EF.Contexts;

namespace RedShift.Infrastructure.Services;

internal sealed class UserReadService : IUserReadService, IAsyncDisposable
{
    private readonly RedShiftReadDbContext _context;
    private readonly IMapper _mapper;

    public UserReadService(RedShiftReadDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<bool> ExistsAsync(string email)
    {
        return _context.Users.AnyAsync(x => x.Email == email);
    }

    public Task<UserDto> GetUserByEmailAsync(string email)
    {
        return _context.Users
            .AsNoTracking()
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}
