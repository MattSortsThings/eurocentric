using System.Data;
using Dapper;
using Eurocentric.Components.DataAccess.EFCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

public sealed partial class TestWebApp
{
    private string _testDbName = string.Empty;
    private string _testId = string.Empty;

    private void SetTestDbName() => _testDbName = "db_" + _testId.Replace("-", string.Empty);

    private async Task CreateTestDbAsync()
    {
        string sql = $"CREATE DATABASE [{_testDbName}]";

        await using SqlConnection dbConnection = new(SharedDbContainer.GetMasterDbConnectionString());
        await dbConnection.OpenAsync().ConfigureAwait(false);
        await dbConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);
        await dbConnection.CloseAsync().ConfigureAwait(false);
    }

    private async Task MigrateTestDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    private async Task DropTestDbAsync()
    {
        string sql = $"""
            ALTER DATABASE [{_testDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            DROP DATABASE [{_testDbName}];
            """;

        await using SqlConnection dbConnection = new(SharedDbContainer.GetMasterDbConnectionString());
        await dbConnection.OpenAsync().ConfigureAwait(false);
        await dbConnection.ExecuteAsync(sql, commandType: CommandType.Text).ConfigureAwait(false);
        await dbConnection.CloseAsync().ConfigureAwait(false);
    }
}
