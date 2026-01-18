using Eurocentric.AcceptanceTests.TestUtils.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

public sealed partial class TestWebApp : WebApplicationFactory<Program>, IAsyncInitializer, ITestWebApp
{
    [ClassDataSource<DbContainer>(Shared = SharedType.PerTestSession)]
    public required DbContainer SingletonDbContainer { get; init; }

    [ClassDataSource<SqlBatchProvider>(Shared = SharedType.PerTestSession)]
    public required SqlBatchProvider SingletonSqlBatchProvider { get; init; }

    public async Task InitializeAsync()
    {
        StartServer();
        await CreateTestDbAsync();
        await MigrateTestDbAsync();
        await CreateTestDbSprocsAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        await DropTestDbAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ReplaceAzureSqlDbSettings(builder);
        builder.ConfigureServices(AddRestClient);
    }
}
