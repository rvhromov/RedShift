using RedShift.Application.Dtos.Users;

namespace RedShift.Application.Services;

public interface IUserReadService
{
    public Task<bool> ExistsAsync(string email);
    public Task<UserDto> GetUserByEmailAsync(string email);
}
