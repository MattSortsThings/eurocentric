using Eurocentric.Components.DataAccess.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

public sealed partial class TestWebApp
{
    /// <inheritdoc />
    public async Task EraseAllDataAsync()
    {
        await ExecuteScopedAsync(static async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            const string sql = "EXECUTE [placeholder].[usp_erase_all_data];";

            await dbContext.Database.ExecuteSqlRawAsync(sql).ConfigureAwait(false);
        });
    }

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
}
