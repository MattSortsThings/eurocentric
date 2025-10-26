using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
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

    public async Task<Result<Contest, IDomainError>> GetByIdAsync(
        ContestId contestId,
        CancellationToken cancellationToken = default
    )
    {
        Contest? contest = await dbContext
            .Contests.AsNoTracking()
            .SingleOrDefaultAsync(contest => contest.Id.Equals(contestId), cancellationToken);

        return contest is not null ? contest : ContestErrors.ContestNotFound(contestId);
    }

    public IQueryable<Contest> GetAsQueryable() => dbContext.Contests.AsNoTracking();
}
