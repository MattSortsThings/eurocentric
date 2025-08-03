using DotNet.Testcontainers.Containers;
using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    public override async Task InitializeAsync()
    {
        await StartDbContainerAndUseConnectionStringAsync();
        EnsureServerStarted();
        await MigrateDbAsync();
    }

    public async Task PauseDbContainerAsync() => await DbContainer.PauseAsync();

    public async Task UnpauseDbContainerAsync()
    {
        if (DbContainer.State == TestcontainersStates.Paused)
        {
            await DbContainer.UnpauseAsync();
        }
    }
}
