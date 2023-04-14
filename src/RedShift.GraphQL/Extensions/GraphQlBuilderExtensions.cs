using HotChocolate.Execution.Configuration;
using RedShift.GraphQL.Mutations;
using RedShift.GraphQL.Queries;
using RedShift.GraphQL.Subscriptions;
using RedShift.GraphQL.TypeConfigs.SkyMarks;
using RedShift.GraphQL.TypeConfigs.Users;
using RedShift.GraphQL.TypeConversions;

namespace RedShift.GraphQL.Extensions;

public static class GraphQlBuilderExtensions
{
    public static IRequestExecutorBuilder AddCustomTypeExtensions(this IRequestExecutorBuilder builder)
    {
        builder
            .AddTypeExtension<UserQueries>()
            .AddTypeExtension<UserMutations>()
            .AddTypeExtension<RegistrationMutations>()
            .AddTypeExtension<SkyMarkMutations>()
            .AddTypeExtension<SkyMarkQueries>()
            .AddTypeExtension<SkyMarkSubscriptions>()
            .AddTypeExtension<FileMutations>();

        return builder;
    }

    public static IRequestExecutorBuilder AddCustomTypes(this IRequestExecutorBuilder builder)
    {
        builder
            .AddType<UserType>()
            .AddType<UserReadType>()
            .AddType<SkyMarkType>()
            .AddType<SkyMarkReadType>();

        return builder;
    }

    public static IRequestExecutorBuilder AddCustomConversions(this IRequestExecutorBuilder builder)
    {
        builder
            .AddSkyMarksConversions()
            .AddUsersConversions();

        return builder;
    }
}