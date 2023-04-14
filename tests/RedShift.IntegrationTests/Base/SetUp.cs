using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedShift.Application.CommandHandlers.Registration.SignUp;
using RedShift.Infrastructure.MapperProfiles;
using RedShift.Infrastructure.QueryHandlers.Users.GetUsers;
using RedShift.IntegrationTests.Extensions;
using System.Transactions;

namespace RedShift.IntegrationTests.Base;

public abstract class SetUp : IAsyncDisposable
{
    private readonly TransactionScope _transaction;

    protected static IConfiguration Configuration { get; private set; }
    protected static ServiceProvider ServiceProvider { get; private set; }

    protected SetUp()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        ServiceProvider = new ServiceCollection()
            .AddConfiguration(Configuration)
            .AddMediatR(m => m.RegisterServicesFromAssemblies(typeof(SignUp).Assembly, typeof(GetUsers).Assembly))
            .AddAutoMapper(typeof(UserProfile))
            .AddServices()
            .AddDbContexts(Configuration)
            .AddRepositories()
            .AddMockedServices()
            .BuildServiceProvider();

        _transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }

    public ValueTask DisposeAsync()
    {
        _transaction.Dispose();

        return ValueTask.CompletedTask;
    }
}
