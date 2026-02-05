using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Tests.Acceptance.Utils.Fixtures;

public sealed partial class TestWebApp
{
    /// <inheritdoc />
    public void ExecuteScoped(Action<IServiceProvider> func)
    {
        using IServiceScope scope = Services.CreateScope();

        func(scope.ServiceProvider);
    }

    /// <inheritdoc />
    public async Task ExecuteScopedAsync(Func<IServiceProvider, Task> func)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        await func(scope.ServiceProvider);
    }

    /// <inheritdoc />
    public async Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> func)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        return await func(scope.ServiceProvider);
    }

    /// <inheritdoc />
    public async Task ResetAsync() => await EraseAllDataInTestDbAsync();
}
