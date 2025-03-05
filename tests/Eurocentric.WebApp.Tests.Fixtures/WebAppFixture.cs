using Eurocentric.DataAccess.EfCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace Eurocentric.WebApp.Tests.Fixtures;

/// <summary>
///     Abstract base class for a web app fixture running on an in-memory server, with a database running inside its own
///     Docker container that is created and disposed for the current test run.
/// </summary>
public abstract class WebAppFixture : WebApplicationFactory<IWebAppAssemblyMarker>, IAsyncLifetime
{
    protected static readonly Uri BaseAddress = new("http://localhost:5055");

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
        MigrateDatabase();
    }

    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Synchronously executes the specified action using this instance's service provider, within a new service scope
    ///     created by the service provider.
    /// </summary>
    /// <param name="action">An action that takes an <see cref="IServiceProvider" /> instance as its input and returns nothing.</param>
    public void ExecuteScoped(Action<IServiceProvider> action)
    {
        using IServiceScope scope = Services.CreateScope();

        action.Invoke(scope.ServiceProvider);
    }

    /// <summary>
    ///     Asynchronously executes the specified function using this instance's service provider, within a new asynchronous
    ///     service scope created by the service provider.
    /// </summary>
    /// <param name="function">
    ///     A function that takes an <see cref="IServiceProvider" /> instance as its input and returns a <see cref="Task" />.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ExecuteScopedAsync(Func<IServiceProvider, Task> function)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        await function.Invoke(scope.ServiceProvider);
    }

    /// <summary>
    ///     Asynchronously executes the specified function using this instance's service provider, within a new asynchronous
    ///     service scope created by the service provider.
    /// </summary>
    /// <param name="function">
    ///     A function that takes an <see cref="IServiceProvider" /> instance as its input and returns a
    ///     <see cref="Task{TResult}" />.
    /// </param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the result returned by the operation.
    /// </returns>
    public async Task<TResult> ExecuteScopedAsync<TResult>(Func<IServiceProvider, Task<TResult>> function)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        return await function.Invoke(scope.ServiceProvider);
    }

    /// <summary>
    ///     Configures the web host.
    /// </summary>
    /// <remarks>A derivative that overrides this method <i>must</i> invoke the base class method.</remarks>
    /// <param name="builder">The web host builder.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureTestServices(services =>
    {
        if (services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)) is { } descriptor)
        {
            services.Remove(descriptor);
        }

        services.AddAppDbContext(_dbContainer.GetConnectionString());
    });

    private void MigrateDatabase()
    {
        using IServiceScope scope = Services.CreateScope();
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();
    }
}
