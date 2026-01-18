using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

/// <summary>
///     A SQL Server instance running inside a Docker container.
/// </summary>
public sealed partial class DbContainer : IAsyncDisposable, IAsyncInitializer
{
    private const string MasterDbName = "master";
    private const string SourceDbName = "source_db";
    private const string MsSqlContainerImage = "mcr.microsoft.com/mssql/server:2022-CU21-ubuntu-22.04";
    private const string MigrateDbSqlPath = "Eurocentric.AcceptanceTests.TestUtils.Scripts.migrate_db.sql";

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder(MsSqlContainerImage).WithCleanUp(true).Build();

    public async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await CreateSourceDbAsync();
        await MigrateSourceDbAsync();
        await BackupSourceDbAsync();
    }

    private string GetMasterDbConnectionString() => _dbContainer.GetConnectionString();

    public string GetNamedDbConnectionString(string dbName) =>
        _dbContainer.GetConnectionString().Replace(MasterDbName, dbName);

    public async Task RestoreNamedDbFromSourceDbBackupAsync(string dbName)
    {
        await using SqlConnection dbConnection = new(GetMasterDbConnectionString());

        await dbConnection.OpenAsync().ConfigureAwait(false);

        string sql = $"""
            RESTORE DATABASE [{dbName}]
            FROM DISK = '/var/opt/mssql/backups/{SourceDbName}.bak'
            WITH MOVE 'source_db' TO '/var/opt/mssql/data/{dbName}.mdf',
                 MOVE 'source_db_log' TO '/var/opt/mssql/data/{dbName}_log.ldf',
                 REPLACE;
            """;

        await dbConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);

        await dbConnection.CloseAsync().ConfigureAwait(false);
    }

    public async Task DropNamedDbAsync(string dbName)
    {
        await using SqlConnection dbConnection = new(GetMasterDbConnectionString());

        await dbConnection.OpenAsync().ConfigureAwait(false);

        string sql = $"""
            ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            DROP DATABASE [{dbName}];
            """;

        await dbConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);

        await dbConnection.CloseAsync().ConfigureAwait(false);
    }

    private async Task CreateSourceDbAsync()
    {
        await using SqlConnection dbConnection = new(GetMasterDbConnectionString());

        await dbConnection.OpenAsync().ConfigureAwait(false);

        await dbConnection
            .ExecuteAsync($"CREATE DATABASE [{SourceDbName}];", commandType: CommandType.Text)
            .ConfigureAwait(false);

        await dbConnection.CloseAsync().ConfigureAwait(false);
    }

    private async Task MigrateSourceDbAsync()
    {
        await using SqlConnection dbConnection = new(GetNamedDbConnectionString(SourceDbName));

        await dbConnection.OpenAsync().ConfigureAwait(false);

        foreach (string sqlBatch in await ReadSqlBatchesFromEmbeddedResourceAsync(MigrateDbSqlPath))
        {
            await dbConnection.ExecuteAsync(sqlBatch, commandType: CommandType.Text).ConfigureAwait(false);
        }

        await dbConnection.CloseAsync().ConfigureAwait(false);
    }

    private async Task BackupSourceDbAsync()
    {
        await using SqlConnection dbConnection = new(GetMasterDbConnectionString());

        await dbConnection.OpenAsync().ConfigureAwait(false);

        const string sql = $"""
            BACKUP DATABASE [{SourceDbName}]
            TO DISK = '/var/opt/mssql/backups/{SourceDbName}.bak'
            WITH INIT, FORMAT;
            """;

        await dbConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);

        await dbConnection.CloseAsync().ConfigureAwait(false);
    }

    private static async Task<string[]> ReadSqlBatchesFromEmbeddedResourceAsync(string resourcePath)
    {
        Assembly assembly = typeof(DbContainer).Assembly;

        await using Stream? stream = assembly.GetManifestResourceStream(resourcePath);

        if (stream is null)
        {
            throw new ArgumentException($"Resource not found: '{resourcePath}'.");
        }

        using StreamReader reader = new(stream);

        string contents = await reader.ReadToEndAsync();

        return SplitOnGoAndFilterBatches(contents);
    }

    private static string[] SplitOnGoAndFilterBatches(string script) =>
        MultiLineGoRegex()
            .Split(script)
            .Select(sql => sql.Trim())
            .Where(sql => !string.IsNullOrWhiteSpace(sql))
            .ToArray();

    [GeneratedRegex(@"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex MultiLineGoRegex();
}
