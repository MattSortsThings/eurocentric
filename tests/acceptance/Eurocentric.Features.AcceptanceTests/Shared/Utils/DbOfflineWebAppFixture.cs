using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

public sealed class DbOfflineWebAppFixture : WebAppFixture
{
    public override async Task InitializeAsync()
    {
        await StartDbContainerAndUseConnectionStringAsync();
        EnsureServerStarted();
        await MigrateDbAsync();
        await PauseDbContainerAsync();
    }

    private async Task PauseDbContainerAsync() => await DbContainer.PauseAsync();
}
