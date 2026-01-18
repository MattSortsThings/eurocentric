using System.Data;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

public sealed partial class TestWebApp
{
    private readonly string _testDbName = $"test_db_{Guid.NewGuid():N}";

    private async Task CreateTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetMasterDbConnectionString());

        await sqlConnection.OpenAsync();

        string sql = $"CREATE DATABASE [{_testDbName}];";

        await sqlConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);

        sqlConnection.Close();
    }

    private async Task MigrateTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetNamedDbConnectionString(_testDbName));

        await sqlConnection.OpenAsync();

        foreach (string sql in SingletonSqlBatchProvider.GetTestDbMigrationSqlBatches())
        {
            await sqlConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);
        }

        sqlConnection.Close();
    }

    private async Task CreateTestDbSprocsAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetNamedDbConnectionString(_testDbName));

        await sqlConnection.OpenAsync();

        foreach (string sql in SingletonSqlBatchProvider.GetTestDbSprocCreationSqlBatches())
        {
            await sqlConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);
        }

        sqlConnection.Close();
    }

    private async Task DropTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetMasterDbConnectionString());

        await sqlConnection.OpenAsync();

        string sql = $"""
            ALTER DATABASE [{_testDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            DROP DATABASE [{_testDbName}];
            """;

        await sqlConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);

        sqlConnection.Close();
    }

    private void ReplaceAzureSqlDbSettings(IWebHostBuilder builder)
    {
        builder.UseSetting("AzureSqlDb:ConnectionString", SingletonDbContainer.GetNamedDbConnectionString(_testDbName));
        builder.UseSetting("AzureSqlDb:MaxRetries", "0");
    }
}
