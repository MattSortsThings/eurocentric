using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

/// <summary>
///     An in-memory test web app using a shared containerized SQL Server instance.
/// </summary>
public sealed partial class TestWebApp
{
    /// <inheritdoc />
    public void ExecuteScoped(Action<IServiceProvider> func)
    {
        using IServiceScope scope = Services.CreateScope();
        func(scope.ServiceProvider);
    }

    /// <inheritdoc />
    public async Task ExecuteScoped(Func<IServiceProvider, Task> func)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await func(scope.ServiceProvider);
    }

    /// <inheritdoc />
    public async Task<T> ExecuteScoped<T>(Func<IServiceProvider, Task<T>> func)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        return await func(scope.ServiceProvider);
    }
}
