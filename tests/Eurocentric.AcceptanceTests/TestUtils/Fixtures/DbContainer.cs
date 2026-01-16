using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

/// <summary>
///     An SQL Server instance running inside a Docker container.
/// </summary>
public sealed class DbContainer : IAsyncInitializer, IAsyncDisposable
{
    private const string MsSqlContainerImage = "mcr.microsoft.com/mssql/server:2022-CU21-ubuntu-22.04";

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder(MsSqlContainerImage).WithCleanUp(true).Build();

    /// <summary>
    ///     Stops and disposes of the container.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync().ConfigureAwait(false);
        await _dbContainer.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Starts the container.
    /// </summary>
    public async Task InitializeAsync() => await _dbContainer.StartAsync().ConfigureAwait(false);

    /// <summary>
    ///     Gets the string for connecting to the <c>"master"</c> database.
    /// </summary>
    /// <returns>An SQL Server database connection string.</returns>
    public string GetMasterDbConnectionString() => _dbContainer.GetConnectionString();

    /// <summary>
    ///     Gets the string for connecting to the named database.
    /// </summary>
    /// <param name="dbName">The name of the database.</param>
    /// <returns>An SQL Server database connection string.</returns>
    public string GetNamedDbConnectionString(string dbName) =>
        _dbContainer.GetConnectionString().Replace("master", dbName);
}
