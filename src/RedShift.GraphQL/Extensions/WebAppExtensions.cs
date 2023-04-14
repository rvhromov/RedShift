using HotChocolate.AspNetCore.Voyager;

namespace RedShift.GraphQL.Extensions;

public static class WebAppExtensions
{
    public static WebApplication UseGraphQL(this WebApplication application)
    {
        application.MapGraphQL("/graphql");
        application.UseVoyager("/graphql", "/graphql-voyager");

        return application;
    }
}
