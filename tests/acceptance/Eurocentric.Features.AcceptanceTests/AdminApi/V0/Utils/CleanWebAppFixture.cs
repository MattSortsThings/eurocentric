using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    public override async Task InitializeAsync()
    {
        await StartDbContainerAndUseConnectionStringAsync();
        EnsureServerStarted();
        await MigrateDbAsync();
    }
}
