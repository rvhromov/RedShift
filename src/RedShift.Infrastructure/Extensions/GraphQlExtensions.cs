using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedShift.Infrastructure.EF.Contexts;
using RedShift.Infrastructure.EF.DataLoaders;

namespace RedShift.Infrastructure.Extensions;

public static class GraphQlExtensions
{
    public static IRequestExecutorBuilder AddGraphQLDbContexts(this IRequestExecutorBuilder executorBuilder)
    {
        executorBuilder
            .RegisterDbContext<RedShiftWriteDbContext>(DbContextKind.Pooled)
            .RegisterDbContext<RedShiftReadDbContext>(DbContextKind.Pooled);

        return executorBuilder;
    }

    public static IRequestExecutorBuilder AddDataLoaders(this IRequestExecutorBuilder executorBuilder)
    {
        executorBuilder
            .AddDataLoader<UserByIdDataLoader>()
            .AddDataLoader<SkyMarkByIdDataLoader>();

        return executorBuilder;
    }
}
