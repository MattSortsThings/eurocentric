using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Contests;
using SlimMessageBus;

namespace Eurocentric.Components.Repositories;

internal sealed class ContestWriteRepository : BaseWriteRepository, IContestWriteRepository
{
    public ContestWriteRepository(IPublishBus publishBus, AppDbContext dbContext)
        : base(publishBus, dbContext) { }

    public void Add(Contest contest) => DbContext.Contests.Add(contest);
}
