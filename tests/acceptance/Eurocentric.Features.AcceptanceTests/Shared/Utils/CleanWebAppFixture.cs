using DotNet.Testcontainers.Containers;
using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    public async Task EnsureDbContainerUnpausedAsync()
    {
        if (DbContainer.State == TestcontainersStates.Paused)
        {
            await DbContainer.UnpauseAsync();
        }
    }
}
