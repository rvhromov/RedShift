using HotChocolate.Data.Filters;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.GraphQL.TypeConfigs.Users;

public sealed class UserFilterInputType : FilterInputType<UserReadModel>
{
    protected override void Configure(IFilterInputTypeDescriptor<UserReadModel> descriptor)
    {
        descriptor.Ignore(f => f.Status);
        descriptor.Ignore(f => f.CreatedAt);
        descriptor.Ignore(f => f.UpdatedAt);
        descriptor.Ignore(f => f.SkyMarks);
        descriptor.Ignore(f => f.Role);
    }
}
