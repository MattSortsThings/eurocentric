using DotNet.Testcontainers.Containers;
using Testcontainers.MsSql;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public sealed class DbContainerToggler(MsSqlContainer dbContainer)
{
    public async Task PauseAsync()
    {
        if (dbContainer.State != TestcontainersStates.Paused)
        {
            await dbContainer.PauseAsync();
        }
    }

    public async Task UnpauseAsync()
    {
        if (dbContainer.State == TestcontainersStates.Paused)
        {
            await dbContainer.UnpauseAsync();
        }
    }
}
