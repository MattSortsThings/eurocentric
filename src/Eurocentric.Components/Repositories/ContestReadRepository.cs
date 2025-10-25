using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Contests;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories;

internal sealed class ContestReadRepository(AppDbContext dbContext) : IContestReadRepository
{
    public async Task<Contest[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Contests.AsNoTracking()
            .OrderBy(contest => contest.ContestYear)
            .ToArrayAsync(cancellationToken);
    }
}
