using Eurocentric.Features.AcceptanceTests.Utils;
using Testcontainers.MsSql;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public sealed class WebAppFixture : WebAppFixtureBase, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithName("acceptance-tests-admin-api-v0")
        .Build();

    private protected override string DbConnectionString => _dbContainer.GetConnectionString();

    public async ValueTask InitializeAsync() => await _dbContainer.StartAsync();

    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}
