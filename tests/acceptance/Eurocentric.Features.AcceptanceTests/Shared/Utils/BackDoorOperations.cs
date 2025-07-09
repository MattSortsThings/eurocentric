using Eurocentric.Features.AcceptanceTests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

internal static class BackDoorOperations
{
    internal static async Task EnsureDatabasePausedAsync(IServiceProvider serviceProvider)
    {
        DbContainerToggler toggler = serviceProvider.GetRequiredService<DbContainerToggler>();

        await toggler.PauseAsync();
    }

    internal static async Task EnsureDatabaseUnpausedAsync(IServiceProvider serviceProvider)
    {
        DbContainerToggler toggler = serviceProvider.GetRequiredService<DbContainerToggler>();

        await toggler.UnpauseAsync();
    }
}
