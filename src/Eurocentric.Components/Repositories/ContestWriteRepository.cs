using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Contests;

namespace Eurocentric.Components.Repositories;

internal sealed class ContestWriteRepository : BaseWriteRepository, IContestWriteRepository
{
    public ContestWriteRepository(AppDbContext dbContext)
        : base(dbContext) { }

    public void Add(Contest contest) => DbContext.Contests.Add(contest);
}
