using RedShift.Domain.Enums;

namespace RedShift.Application.Dtos.Users;

public sealed class UserDto
{
    public Guid Id { get; set; }
    public RoleType Role { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
}
