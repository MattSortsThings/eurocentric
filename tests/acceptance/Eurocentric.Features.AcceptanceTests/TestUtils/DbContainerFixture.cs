using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public sealed class DbContainerFixture : IAsyncInitializer, IAsyncDisposable
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    public async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public string GetConnectionString() => _dbContainer.GetConnectionString();
}
