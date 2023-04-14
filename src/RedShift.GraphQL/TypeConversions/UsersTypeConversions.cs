using HotChocolate.Execution.Configuration;
using RedShift.Domain.ValueObjects.Users;

namespace RedShift.GraphQL.TypeConversions;

public static class UsersTypeConversions
{
    public static IRequestExecutorBuilder AddUsersConversions(this IRequestExecutorBuilder builder)
    {
        builder.AddTypeConverter<Email, string>(x => x.Value);

        return builder;
    }
}
