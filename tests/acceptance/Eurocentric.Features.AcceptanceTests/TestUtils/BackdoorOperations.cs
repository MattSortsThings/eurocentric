using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public static class BackdoorOperations
{
    public static async Task EraseAllDataAsync(IServiceProvider serviceProvider)
    {
        await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        await dbContext.V0Broadcasts.ExecuteDeleteAsync();
        await dbContext.V0Contests.ExecuteDeleteAsync();
        await dbContext.V0Countries.ExecuteDeleteAsync();
    }
}
