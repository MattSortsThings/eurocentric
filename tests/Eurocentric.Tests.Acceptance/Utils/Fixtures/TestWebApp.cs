using Eurocentric.Tests.Acceptance.Utils.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TUnit.Core.Interfaces;

namespace Eurocentric.Tests.Acceptance.Utils.Fixtures;

/// <summary>
///     An in-memory test web application using its own test database.
/// </summary>
public sealed partial class TestWebApp : WebApplicationFactory<Program>, IAsyncInitializer, ITestWebApp
{
    /// <summary>
    ///     A singleton Microsoft SQL Server instance running inside a Docker container.
    /// </summary>
    [ClassDataSource<DbContainer>(Shared = SharedType.PerTestSession)]
    public required DbContainer SingletonDbContainer { get; init; }

    /// <summary>
    ///     Starts the server, then creates the test database.
    /// </summary>
    public async Task InitializeAsync()
    {
        StartServer();
        await CreateTestDbAsync();
    }

    /// <summary>
    ///     Drops the test database, then stops the server.
    /// </summary>
    public override async ValueTask DisposeAsync()
    {
        await DropTestDbAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureServices(AddRestClient);
}
