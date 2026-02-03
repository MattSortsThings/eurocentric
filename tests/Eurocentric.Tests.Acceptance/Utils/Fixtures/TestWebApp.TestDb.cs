using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Eurocentric.Tests.Acceptance.Utils.Fixtures;

public sealed partial class TestWebApp
{
    private string TestDbName { get; } = "db_" + Guid.NewGuid().ToString("D").Replace('-', '_');

    private async Task CreateTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(SingletonDbContainer.GetMasterDbConnectionString());

        await sqlConnection.OpenAsync();

        string sql = $"CREATE DATABASE [{TestDbName}];";

        await sqlConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);

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
}
