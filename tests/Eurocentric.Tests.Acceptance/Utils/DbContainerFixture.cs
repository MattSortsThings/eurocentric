using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.Tests.Acceptance.Utils;

/// <summary>
///     A Microsoft SQL Server instance running inside a container.
/// </summary>
public sealed class DbContainerFixture : IAsyncDisposable, IAsyncInitializer
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    /// <summary>
    ///     Asynchronously stops the container, then disposes of it.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    /// <summary>
    ///     Asynchronously starts the container.
    /// </summary>
    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    /// <summary>
    ///     Gets a database connection string for the container.
    /// </summary>
    /// <param name="dbName">
    ///     Specifies the database name. Database name defaults to <c>"master"</c> when
    ///     <paramref name="dbName" /> is <see langword="null" />.
    /// </param>
    /// <returns>A Microsoft SQL Server database connection string.</returns>
    public string GetConnectionString(string? dbName = null)
    {
        return dbName is null
            ? _dbContainer.GetConnectionString()
            : _dbContainer.GetConnectionString().Replace("master", dbName);
    }
}
