using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace Eurocentric.WebApp.Tests.Fixtures;

/// <summary>
///     Abstract base class for a web app fixture running on an in-memory server, with a database running inside its own
///     Docker container that is created and disposed for the current test run.
/// </summary>
public abstract class WebAppFixture : WebApplicationFactory<IWebApiAssemblyMarker>, IAsyncLifetime
{
    protected static readonly Uri BaseAddress = new("http://localhost:5116");

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    public async ValueTask InitializeAsync() => await _dbContainer.StartAsync();

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
    protected void ExecuteScoped(Action<IServiceProvider> action)
    {
        using IServiceScope scope = Services.CreateScope();

        action.Invoke(scope.ServiceProvider);
    }

    /// <summary>
    ///     Asynchronously executes the specified function using this instance's service provider, within a new asynchronous
    ///     service scope created by the service provider.
    /// </summary>
    /// <param name="function">
    ///     A function that takes an <see cref="IServiceProvider" /> instance as its input and returns a completed task.
    /// </param>
    /// <returns>A completed task.</returns>
    protected async Task ExecuteScopedAsync(Func<IServiceProvider, Task> function)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        await function.Invoke(scope.ServiceProvider);
    }

    /// <summary>
    ///     Asynchronously executes the specified function using this instance's service provider, within a new asynchronous
    ///     service scope created by the service provider.
    /// </summary>
    /// <param name="function">
    ///     A function that takes an <see cref="IServiceProvider" /> instance as its input and returns an instance of type
    ///     <typeparamref name="TResult" /> as a completed task.
    /// </param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>A completed task.</returns>
    protected async Task<TResult> ExecuteScopedAsync<TResult>(Func<IServiceProvider, Task<TResult>> function)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        return await function.Invoke(scope.ServiceProvider);
    }
}
