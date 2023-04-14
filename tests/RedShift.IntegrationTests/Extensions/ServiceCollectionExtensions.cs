using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RedShift.Application.Services;
using RedShift.Domain.Repositories;
using RedShift.Infrastructure.EF.Contexts;
using RedShift.Infrastructure.EF.Repositories;
using RedShift.Infrastructure.Services;

namespace RedShift.IntegrationTests.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserReadService, UserReadService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres");

        services.AddDbContext<RedShiftWriteDbContext>(context => context.UseNpgsql(connectionString).LogTo(Console.WriteLine));
        services.AddDbContext<RedShiftReadDbContext>(context => context.UseNpgsql(connectionString).LogTo(Console.WriteLine));

        services.AddDbContextFactory<RedShiftReadDbContext>(context => context
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISkyMarkRepository, SkyMarkRepository>();

        return services;
    }

    public static IServiceCollection AddMockedServices(this IServiceCollection services)
    {
        services.AddSingleton(x => Substitute.For<IIdentityAccessor>());

        return services;
    }
}
