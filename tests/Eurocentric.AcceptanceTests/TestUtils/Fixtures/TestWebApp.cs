using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

/// <summary>
///     An in-memory test web app using a shared containerized SQL Server instance.
/// </summary>
public sealed partial class TestWebApp : WebApplicationFactory<Program>, IAsyncInitializer, ITestWebApp
{
    [ClassDataSource<DbContainer>(Shared = SharedType.PerTestSession)]
    public required DbContainer SharedDbContainer { get; init; }

    public async Task InitializeAsync()
    {
        SetTestId();
        SetTestDbName();
        StartServer();
        await CreateTestDbAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        await DropTestDbAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("AzureSqlDb:ConnectionString", SharedDbContainer.GetNamedDbConnectionString(_testDbName));
        builder.UseSetting("AzureSqlDb:MaxRetries", "0");
        builder.ConfigureServices(AddRestClient);
    }

    private void SetTestId() => _testId = TestContext.Current!.Id;
}
