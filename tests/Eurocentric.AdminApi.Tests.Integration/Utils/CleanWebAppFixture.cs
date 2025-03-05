using Eurocentric.DataAccess.EfCore;
using Eurocentric.WebApp.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
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
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Countries.ExecuteDelete();
    }
}
