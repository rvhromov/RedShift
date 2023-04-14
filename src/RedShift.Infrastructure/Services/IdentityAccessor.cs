using Microsoft.AspNetCore.Http;
using RedShift.Application.Services;
using RedShift.Domain.Enums;
using RedShift.Domain.Exceptions;
using System.Security.Claims;

namespace RedShift.Infrastructure.Services;

internal sealed class IdentityAccessor : IIdentityAccessor
{
    private readonly ClaimsPrincipal _user;

    public IdentityAccessor(IHttpContextAccessor contextAccessor)
    {
        _user = contextAccessor.HttpContext.User;
    }

    public Guid GetUserId()
    {
        var id = _user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (id is null)
        {
            throw new ValidationException("Unable to get Id value from claims. Id is empty.");
        }

        return Guid.Parse(id);
    }

    public RoleType GetUserRole()
    {
        var role = _user.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ValidationException("Unable to get Role value from claims. Role is empty.");
        }

        return Enum.Parse<RoleType>(role, true);
    }

    public (Guid id, RoleType role) GetUserIdAndRole() => (GetUserId(), GetUserRole());
}
