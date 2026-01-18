using Testcontainers.MsSql;
using TUnit.Core.Interfaces;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

public sealed class DbContainer : IAsyncInitializer, IAsyncDisposable
{
    private const string MasterDbName = "master";
    private const string MsSqlContainerImage = "mcr.microsoft.com/mssql/server:2022-CU21-ubuntu-22.04";

    private MsSqlContainer? _container;

    public async ValueTask DisposeAsync()
    {
        if (_container != null)
        {
            await _container.StopAsync();
            await _container.DisposeAsync();
        }

        _container = null;
    }

    public async Task InitializeAsync()
    {
        MsSqlContainer dbContainer = new MsSqlBuilder(MsSqlContainerImage).WithCleanUp(true).Build();

        await dbContainer.StartAsync();

        _container = dbContainer;
    }

    public string GetMasterDbConnectionString() => _container!.GetConnectionString();

    public string GetNamedDbConnectionString(string dbName) =>
        _container!.GetConnectionString().Replace(MasterDbName, dbName);
}
