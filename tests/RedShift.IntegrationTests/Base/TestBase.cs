using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RedShift.Domain.Abstractions;
using RedShift.Infrastructure.EF.Contexts;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.IntegrationTests.Base;

public abstract class TestBase : SetUp
{
    public static async Task SendAsync(IRequest request)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();

        var mediator = scope.ServiceProvider.GetService<IMediator>();

        await mediator.Send(request);
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();

        var mediator = scope.ServiceProvider.GetService<IMediator>();

        return await mediator.Send(request);
    }

    public static async Task<T> GetEntityAsync<T>(Guid id) where T : BaseReadModel
    {
        await using var scope = ServiceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<RedShiftReadDbContext>();

        return await context
            .Set<T>()
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id);
    } 

    public static async Task<T> AddEntityAsync<T>(T entity) where T : Entity
    {
        await using var scope = ServiceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<RedShiftWriteDbContext>();

        var result = await context
            .Set<T>()
            .AddAsync(entity);

        return result.Entity;
    }

    public static T GetMockedService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}
