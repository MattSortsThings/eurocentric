using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories;

internal sealed class ContestRepository(AppDbContext dbContext) : IContestRepository
{
    public async Task<Result<Contest, IDomainError>> GetUntrackedAsync(
        ContestId id,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id);

        Contest? contest = await dbContext
            .Contests.AsNoTracking()
            .SingleOrDefaultAsync(contest => contest.Id == id, cancellationToken);

        return contest is not null ? contest : ContestErrors.ContestNotFound(id);
    }

    public async Task<Contest[]> GetAllUntrackedAsync<TKey>(
        Expression<Func<Contest, TKey>> sortKey,
        CancellationToken cancellationToken = default
    ) => await dbContext.Contests.AsNoTracking().OrderBy(sortKey).ToArrayAsync(cancellationToken);

    public IQueryable<Contest> GetUntrackedQueryable() => dbContext.Contests.AsNoTracking();

    public void Add(Contest aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Contests.Add(aggregate);
    }

    public async Task<Result<Contest, IDomainError>> GetTrackedAsync(
        ContestId id,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id);

        Contest? contest = await dbContext.Contests.SingleOrDefaultAsync(
            contest => contest.Id == id,
            cancellationToken
        );

        return contest is not null ? contest : ContestErrors.ContestNotFound(id);
    }

    public void Remove(Contest aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Contests.Remove(aggregate);
    }

    public void Update(Contest aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Contests.Update(aggregate);
    }
}
