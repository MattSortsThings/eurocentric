using System.Data;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Respawn;

namespace Eurocentric.Tests.Acceptance.Utils.Fixtures;

public sealed partial class TestWebApp
{
    private Respawner? TestDbRespawner { get; set; }

    private string TestDbName { get; } = "db_" + Guid.NewGuid().ToString("D").Replace('-', '_');

    private async Task CreateTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetMasterDbConnectionString());

        await sqlConnection.OpenAsync();

        string sql = $"CREATE DATABASE [{TestDbName}];";

        await sqlConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);

        sqlConnection.Close();
    }

    private async Task MigrateTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetNamedDbConnectionString(TestDbName));

        await sqlConnection.OpenAsync();

        foreach (string sqlBatch in SingletonSqlBatchProvider.GetMigrationSqlBatches())
        {
            await sqlConnection.ExecuteAsync(sqlBatch, commandType: CommandType.Text).ConfigureAwait(false);
        }

        sqlConnection.Close();
    }

    private async Task AugmentTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetNamedDbConnectionString(TestDbName));

        await sqlConnection.OpenAsync();

        foreach (string sqlBatch in SingletonSqlBatchProvider.GetAugmentationSqlBatches())
        {
            await sqlConnection.ExecuteAsync(sqlBatch, commandType: CommandType.Text).ConfigureAwait(false);
        }

        sqlConnection.Close();
    }

    private async Task CreateTestDbRespawnerAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetNamedDbConnectionString(TestDbName));

        await sqlConnection.OpenAsync();

        TestDbRespawner = await Respawner.CreateAsync(sqlConnection);

        sqlConnection.Close();
    }

    private async Task DropTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetMasterDbConnectionString());

        await sqlConnection.OpenAsync();

        string sql = $"""
            ALTER DATABASE [{TestDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            DROP DATABASE [{TestDbName}];
            """;

        await sqlConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);

        sqlConnection.Close();
    }

    private async Task RespawnTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetNamedDbConnectionString(TestDbName));

        await sqlConnection.OpenAsync();

        await TestDbRespawner!.ResetAsync(sqlConnection);

        sqlConnection.Close();
    }

    private void ReplaceDbConnectionSettings(IWebHostBuilder builder)
    {
        builder.UseSetting("DbConnection:MaxRetries", "0");

        builder.UseSetting(
            "DbConnection:ConnectionString",
            SingletonDbContainer.GetNamedDbConnectionString(TestDbName)
        );
    }
}
