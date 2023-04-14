using RedShift.Domain.Enums;

namespace RedShift.Application.Services;

public interface IIdentityAccessor
{
    Guid GetUserId();
    RoleType GetUserRole();
    (Guid id, RoleType role) GetUserIdAndRole();
}
