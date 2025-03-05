using Eurocentric.DataAccess.InMemory;
using Eurocentric.WebApp.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi.Tests.Integration.Utils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    /// <summary>
    ///     Resets the web app fixture to its initial state.
    /// </summary>
    public void Reset()
    {
        using IServiceScope scope = Services.CreateScope();
        InMemoryRepository? inMemoryRepository = scope.ServiceProvider.GetRequiredService<InMemoryRepository>();

        inMemoryRepository.Countries.Clear();
    }
}
