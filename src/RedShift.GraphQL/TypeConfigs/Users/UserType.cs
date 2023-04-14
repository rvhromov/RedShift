using RedShift.Domain.Entities;
using RedShift.Domain.Enums;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.GraphQL.TypeConfigs.Users;

public sealed class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        var adminRole = RoleType.Admin.ToString();

        descriptor.Ignore(f => f.Password);
        descriptor.Ignore(f => f.Salt);

        descriptor.Field(f => f.Email).Type<StringType>();

        descriptor.Field(f => f.CreatedAt).Authorize(roles: adminRole);
        descriptor.Field(f => f.UpdatedAt).Authorize(roles: adminRole);
        descriptor.Field(f => f.Status).Authorize(roles: adminRole);
    }
}

public sealed class UserReadType : ObjectType<UserReadModel>
{
    protected override void Configure(IObjectTypeDescriptor<UserReadModel> descriptor)
    {
        var adminRole = RoleType.Admin.ToString();

        descriptor.Ignore(f => f.Password);
        descriptor.Ignore(f => f.Salt);

        descriptor.Field(f => f.CreatedAt).Authorize(roles: adminRole);
        descriptor.Field(f => f.UpdatedAt).Authorize(roles: adminRole);
        descriptor.Field(f => f.Status).Authorize(roles: adminRole);
    }
}
