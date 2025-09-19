using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public sealed class DbContainerFixture : IAsyncInitializer, IAsyncDisposable
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    public async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public string GetConnectionString()
    {
        string dbConnectionString = _dbContainer.GetConnectionString();

        return dbConnectionString.TrimEnd(';') + ";Connect Timeout=1;";
    }
}
