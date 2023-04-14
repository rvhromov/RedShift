using HotChocolate.Authorization;
using RedShift.Domain.Enums;

namespace RedShift.GraphQL.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
public sealed class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(RoleType roles)
    {
        Roles = roles
            .ToString()
            .Split(',')
            .Select(r => r.Trim())
            .Distinct()
            .ToArray();
    }
}
