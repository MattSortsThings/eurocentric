using DotNet.Testcontainers.Containers;
using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     A SQL Server database running inside a test container.
/// </summary>
public sealed class DbContainerFixture : IAsyncInitializer, IAsyncDisposable
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    /// <summary>
    ///     Gets the database connection string.
    /// </summary>
    public string ConnectionString { get; private set; } = string.Empty;

    /// <summary>
    ///     Asynchronously disposes of the fixture by stopping the test container.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    /// <summary>
    ///     Asynchronously initializes the fixture by starting the test container then setting the
    ///     <see cref="ConnectionString" /> property.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        ConnectionString = _dbContainer.GetConnectionString().TrimEnd(';') + ";Command Timeout=1";
    }

    /// <summary>
    ///     Asynchronously pauses the test container.
    /// </summary>
    public async Task PauseAsync() => await _dbContainer.PauseAsync();

    /// <summary>
    ///     Asynchronously unpauses the test container if it is currently paused.
    /// </summary>
    public async Task EnsureUnpausedAsync()
    {
        if (_dbContainer.State == TestcontainersStates.Paused)
        {
            await _dbContainer.UnpauseAsync();
        }
    }
}
