using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedShift.Application.CommandHandlers.Registration.SignUp;
using RedShift.Application.Providers;
using RedShift.Application.Services;
using RedShift.Domain.Repositories;
using RedShift.Infrastructure.EF.Contexts;
using RedShift.Infrastructure.EF.Repositories;
using RedShift.Infrastructure.MapperProfiles;
using RedShift.Infrastructure.Providers;
using RedShift.Infrastructure.QueryHandlers.Users.GetUsers;
using RedShift.Infrastructure.Services;

namespace RedShift.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbConfig(configuration)
            .AddRepositories()
            .AddServices()
            .AddProviders()
            .AddMediatR(m => m.RegisterServicesFromAssemblies(typeof(SignUp).Assembly, typeof(GetUsers).Assembly))
            .AddAutoMapper(typeof(UserProfile));

        return services;
    }

    #region Private Helpers

    private static IServiceCollection AddDbConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres");

        services.AddPooledDbContextFactory<RedShiftWriteDbContext>(context => context
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine));
        
        services.AddPooledDbContextFactory<RedShiftReadDbContext>(context => context
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository>(x =>
            new UserRepository(x
                .GetRequiredService<IDbContextFactory<RedShiftWriteDbContext>>()
                .CreateDbContext()));

        services.AddScoped<ISkyMarkRepository>(x =>
            new SkyMarkRepository(x
                .GetRequiredService<IDbContextFactory<RedShiftWriteDbContext>>()
                .CreateDbContext()));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserReadService>(x =>
            new UserReadService(
                x.GetRequiredService<IDbContextFactory<RedShiftReadDbContext>>().CreateDbContext(), 
                x.GetRequiredService<IMapper>()));

        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IIdentityAccessor, IdentityAccessor>();

        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IBlobProvider, BlobProvider>();

        return services;
    }

    #endregion
}
