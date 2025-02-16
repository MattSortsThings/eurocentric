using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace Eurocentric.WebApp.Tests.Fixtures;

public abstract class WebAppFixture : WebApplicationFactory<IWebAppAssemblyMarker>, IAsyncLifetime
{
    protected static readonly Uri BaseUri = new("http://localhost:5056");

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    public async ValueTask InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected void ExecuteScoped(Action<IServiceProvider> action)
    {
        using IServiceScope scope = Services.CreateScope();

        action.Invoke(scope.ServiceProvider);
    }

    protected async Task ExecuteScopedAsync(Func<IServiceProvider, Task> action)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        await action.Invoke(scope.ServiceProvider);
    }

    protected async Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        return await action.Invoke(scope.ServiceProvider);
    }
}
