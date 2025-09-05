using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Contests;

public static class BackdoorOperations
{
    public static Func<IServiceProvider, Task> PersistContestAsync(Contest contest)
    {
        Contest contestToPersist = contest;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            dbContext.Contests.Add(contestToPersist);
            await dbContext.SaveChangesAsync();
        };
    }

    public static Func<IServiceProvider, Task> DeleteContestAsync(ContestId contestId)
    {
        ContestId idToDelete = contestId;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Contests.Where(contest => contest.Id == idToDelete).ExecuteDeleteAsync();
        };
    }
}
