using RedShift.Domain.Entities;
using RedShift.Domain.Enums;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.GraphQL.TypeConfigs.SkyMarks;

public sealed class SkyMarkType : ObjectType<SkyMark>
{
    protected override void Configure(IObjectTypeDescriptor<SkyMark> descriptor)
    {
        var adminRole = RoleType.Admin.ToString();

        descriptor.Field(f => f.Location).Type<StringType>();

        descriptor.Field(f => f.CreatedAt).Authorize(roles: adminRole);
        descriptor.Field(f => f.UpdatedAt).Authorize(roles: adminRole);
        descriptor.Field(f => f.Status).Authorize(roles: adminRole);
    }
}

public sealed class SkyMarkReadType : ObjectType<SkyMarkReadModel>
{
    protected override void Configure(IObjectTypeDescriptor<SkyMarkReadModel> descriptor)
    {
        var adminRole = RoleType.Admin.ToString();

        descriptor.Field(f => f.Location).Type<StringType>();

        descriptor.Field(f => f.CreatedAt).Authorize(roles: adminRole);
        descriptor.Field(f => f.UpdatedAt).Authorize(roles: adminRole);
        descriptor.Field(f => f.Status).Authorize(roles: adminRole);
    }
}
