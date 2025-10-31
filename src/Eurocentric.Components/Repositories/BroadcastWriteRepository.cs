using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Broadcasts;
using SlimMessageBus;

namespace Eurocentric.Components.Repositories;

internal sealed class BroadcastWriteRepository : BaseWriteRepository, IBroadcastWriteRepository
{
    public BroadcastWriteRepository(IPublishBus publishBus, AppDbContext dbContext)
        : base(publishBus, dbContext) { }

    public void Add(Broadcast contest) => DbContext.Broadcasts.Add(contest);
}
