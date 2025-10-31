using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Components.Repositories;

internal sealed class ContestWriteRepository : BaseWriteRepository, IContestWriteRepository
{
    public ContestWriteRepository(IPublishBus publishBus, AppDbContext dbContext)
        : base(publishBus, dbContext) { }

    public void Add(Contest contest) => DbContext.Contests.Add(contest);

    public void Update(Contest contest) => DbContext.Contests.Update(contest);

    public async Task<Result<Contest, IDomainError>> GetByIdAsync(
        ContestId contestId,
        CancellationToken cancellationToken = default
    )
    {
        Contest? contest = await DbContext.Contests.SingleOrDefaultAsync(
            contest => contest.Id.Equals(contestId),
            cancellationToken
        );

        return contest is not null ? contest : ContestErrors.ContestNotFound(contestId);
    }
}
