using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;

public sealed class WebAppFixture : WebAppFixtureBase
{
    public void Reset()
    {
        Action<IServiceProvider> eraseAllData = sp =>
        {
            using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            dbContext.Countries.ExecuteDelete();
        };

        ExecuteScoped(eraseAllData);
    }
}
