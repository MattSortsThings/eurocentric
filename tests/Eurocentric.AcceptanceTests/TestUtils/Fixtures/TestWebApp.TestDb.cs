using Microsoft.AspNetCore.Hosting;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

public sealed partial class TestWebApp
{
    private string TestDbName { get; set; } = string.Empty;

    private void SetTestDbNameFromTestId() => TestDbName = "test_db_" + TestId.Replace("-", "_");

    private void ReplaceAzureSqlDbSettings(IWebHostBuilder builder)
    {
        builder.UseSetting("AzureSqlDb:ConnectionString", SingletonDbContainer.GetNamedDbConnectionString(TestDbName));
        builder.UseSetting("AzureSqlDb:MaxRetries", "0");
    }

    private async Task CreateTestDbAsync() =>
        await SingletonDbContainer.RestoreNamedDbFromSourceDbBackupAsync(TestDbName);

    private async Task DropTestDbAsync() => await SingletonDbContainer.DropNamedDbAsync(TestDbName);
}
