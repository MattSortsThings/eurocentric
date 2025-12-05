using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.Tests.Acceptance.Utils;

/// <summary>
///     A Microsoft SQL Server instance running inside a container.
/// </summary>
public sealed class MsSqlServerFixture : IAsyncInitializer, IAsyncDisposable
{
    private const string DefaultDatabaseName = "master";
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    /// <summary>
    ///     Asynchronously disposes of the fixture by stopping then disposing of the container.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    /// <summary>
    ///     Asynchronously initializes the fixture by starting the container.
    /// </summary>
    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    /// <summary>
    ///     Gets the default connection string for the container.
    /// </summary>
    /// <returns>An MS SQL Server database connection string.</returns>
    public string GetConnectionString() => _dbContainer.GetConnectionString();

    /// <summary>
    ///     Gets the connection string for the container using the specified database name.
    /// </summary>
    /// <param name="dbName">The database name.</param>
    /// <returns>An MS SQL Server database connection string.</returns>
    public string GetConnectionString(string dbName) =>
        _dbContainer.GetConnectionString().Replace(DefaultDatabaseName, dbName);
}
