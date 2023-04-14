using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RedShift.Application.Helpers;
using RedShift.GraphQL.Filters;
using RedShift.GraphQL.Middlewares;
using RedShift.Infrastructure.Extensions;
using RedShift.Infrastructure.Options;
using StackExchange.Redis;
using System.Text;

namespace RedShift.GraphQL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQL(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("redis");

        services
            .AddGraphQLServer()
            .AddRedisSubscriptions(provider => ConnectionMultiplexer.Connect(redisConnection))
            .AddSocketSessionInterceptor<SubscriptionAuthMiddleware>()
            .AddGraphQLDbContexts()
            .AddDataLoaders()
            .AddDefaultTransactionScopeHandler()
            .AddAuthorization()
            .AddErrorFilter(provider => new ExceptionFilter())
            .AddQueryType()
            .AddMutationType()
            .AddSubscriptionType()
            .AddProjections()
            .AddFiltering()
            .AddSorting()
            .AddCustomTypeExtensions()
            .AddCustomTypes()
            .AddCustomConversions();

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetOptions<JwtOptions>(nameof(JwtOptions));
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));

        var jwtBearerOptions = new JwtBearerOptions
        {
            SaveToken = true,
            TokenValidationParameters = new()
            {
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = signingKey,
                ValidAudience = jwtOptions.Audience,
                ValidIssuer = jwtOptions.Issuer,
                ValidateIssuerSigningKey = jwtOptions.ValidateIssuer,
                ValidateAudience = jwtOptions.ValidateAudience,
                ValidateIssuer = jwtOptions.ValidateIssuer,
                ValidateLifetime = jwtOptions.ValidateLifetime
            }
        };

        services.AddSingleton(jwtBearerOptions);

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = jwtBearerOptions.SaveToken;
                options.TokenValidationParameters = jwtBearerOptions.TokenValidationParameters;
            });

        return services;
    }
}
