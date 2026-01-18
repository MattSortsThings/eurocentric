using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

public sealed partial class TestWebApp : WebApplicationFactory<Program>, IAsyncInitializer, ITestWebApp
{
    [ClassDataSource<DbContainer>(Shared = SharedType.PerTestSession)]
    public required DbContainer SingletonDbContainer { get; init; }

    private string TestId { get; set; } = string.Empty;

    public async Task InitializeAsync()
    {
        SetTestId();
        SetTestDbNameFromTestId();
        StartServer();
        await CreateTestDbAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        await DropTestDbAsync();
        await base.DisposeAsync();
    }

    private void SetTestId() => TestId = TestContext.Current?.Id ?? Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ReplaceAzureSqlDbSettings(builder);
        builder.ConfigureServices(AddRestClient);
    }
}
