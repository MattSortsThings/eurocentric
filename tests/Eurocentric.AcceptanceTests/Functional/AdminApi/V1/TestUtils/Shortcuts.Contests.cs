using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.Contest;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async Task<ContestDto> CreateALiverpoolRulesContestAsync(
        this AdminKernel kernel,
        int contestYear,
        string cityName
    )
    {
        LiverpoolRulesContest contest = LiverpoolRulesContest.CreateDummyContest(Guid.NewGuid(), contestYear, cityName);

        ContestDto contestDto = contest.ToDto();

        await kernel.BackDoor.ExecuteScopedAsync(PersistAsync(contest));

        return contestDto;
    }

    public static async Task<ContestDto> CreateAStockholmRulesContestAsync(
        this AdminKernel kernel,
        int contestYear,
        string cityName
    )
    {
        StockholmRulesContest contest = StockholmRulesContest.CreateDummyContest(Guid.NewGuid(), contestYear, cityName);

        ContestDto contestDto = contest.ToDto();

        await kernel.BackDoor.ExecuteScopedAsync(PersistAsync(contest));

        return contestDto;
    }

    public static async Task DeleteAContestAsync(this AdminKernel kernel, Guid contestId)
    {
        ContestId id = ContestId.FromValue(contestId);

        await kernel.BackDoor.ExecuteScopedAsync(DeleteAsync(id));
    }

    private static Func<IServiceProvider, Task> DeleteAsync(ContestId contestId)
    {
        ContestId idToDelete = contestId;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Contests.Where(contest => contest.Id.Equals(idToDelete)).ExecuteDeleteAsync();
        };
    }

    private static Func<IServiceProvider, Task> PersistAsync(ContestAggregate aggregate)
    {
        ContestAggregate aggToPersist = aggregate;

        return async serviceProvider =>
        {
            await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            dbContext.Contests.Add(aggToPersist);
            await dbContext.SaveChangesAsync();
        };
    }
}
