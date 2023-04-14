using HotChocolate.Data.Filters;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.GraphQL.TypeConfigs.SkyMarks;

public sealed class SkyMarkFilterInputType : FilterInputType<SkyMarkReadModel>
{
    protected override void Configure(IFilterInputTypeDescriptor<SkyMarkReadModel> descriptor)
    {
        descriptor.Ignore(f => f.Status);
        descriptor.Ignore(f => f.CreatedAt);
        descriptor.Ignore(f => f.UpdatedAt);
        descriptor.Ignore(f => f.User);
        descriptor.Ignore(f => f.Description);
        descriptor.Ignore(f => f.Image);
    }
}