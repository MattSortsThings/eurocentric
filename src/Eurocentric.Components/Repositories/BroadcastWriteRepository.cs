using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Components.Repositories;

internal sealed class BroadcastWriteRepository : BaseWriteRepository, IBroadcastWriteRepository
{
    public BroadcastWriteRepository(IPublishBus publishBus, AppDbContext dbContext)
        : base(publishBus, dbContext) { }

    public void Add(Broadcast contest) => DbContext.Broadcasts.Add(contest);

    public void Update(Broadcast broadcast) => DbContext.Broadcasts.Update(broadcast);

    public void Remove(Broadcast broadcast) => DbContext.Broadcasts.Remove(broadcast);

    public async Task<Result<Broadcast, IDomainError>> GetByIdAsync(
        BroadcastId broadcastId,
        CancellationToken cancellationToken = default
    )
    {
        Broadcast? broadcast = await DbContext.Broadcasts.SingleOrDefaultAsync(
            broadcast => broadcast.Id.Equals(broadcastId),
            cancellationToken
        );

        return broadcast is not null ? broadcast : BroadcastErrors.BroadcastNotFound(broadcastId);
    }
}
