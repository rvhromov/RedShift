using RedShift.Domain.Enums;

namespace RedShift.Application.Services;

public interface ITokenService
{
    public string CreateAccessToken(Guid userId, RoleType userRole);
}
