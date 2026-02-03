using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.Tests.Acceptance.Utils.Fixtures;

/// <summary>
///     A Microsoft SQL Server instance running inside a Docker container.
/// </summary>
public sealed class DbContainer : IAsyncInitializer, IAsyncDisposable
{
    private const string MasterDbName = "master";
    private const string MsSqlContainerImage = "mcr.microsoft.com/mssql/server:2022-CU21-ubuntu-22.04";

    private MsSqlContainer? _container;

    /// <summary>
    ///     Stops the container then disposes of it.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_container != null)
        {
            await _container.StopAsync();
            await _container.DisposeAsync();
        }

        _container = null;
    }

    /// <summary>
    ///     Starts the container.
    /// </summary>
    public async Task InitializeAsync()
    {
        MsSqlContainer dbContainer = new MsSqlBuilder(MsSqlContainerImage).WithCleanUp(true).Build();

        await dbContainer.StartAsync();

        _container = dbContainer;
    }

    /// <summary>
    ///     Gets the string for connecting to the "master" database on the server.
    /// </summary>
    /// <returns>A Microsoft SQL server database connection string.</returns>
    public string GetMasterDbConnectionString() => _container!.GetConnectionString();

    /// <summary>
    ///     Gets the string for connecting to a named database on the server.
    /// </summary>
    /// <param name="dbName">The name of the required database.</param>
    /// <returns>A Microsoft SQL server database connection string.</returns>
    public string GetNamedDbConnectionString(string dbName) =>
        _container!.GetConnectionString().Replace(MasterDbName, dbName);
}
