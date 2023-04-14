using HotChocolate.Execution.Configuration;
using Location = RedShift.Domain.ValueObjects.Location.Location;

namespace RedShift.GraphQL.TypeConversions;

public static class SkyMarksTypeConversions
{
    public static IRequestExecutorBuilder AddSkyMarksConversions(this IRequestExecutorBuilder builder)
    {
        builder.AddTypeConverter<Location, string>(x =>
        {
            var (rightAscension, declination) = x.ToDmsCoordinates();

            return $"{rightAscension}, {declination}";
        });

        return builder;
    }
}
