using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories;

internal sealed class BroadcastRepository(AppDbContext dbContext) : IBroadcastRepository
{
    public async Task<Result<Broadcast, IDomainError>> GetUntrackedAsync(
        BroadcastId id,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id);

        Broadcast? broadcast = await dbContext
            .Broadcasts.AsNoTracking()
            .SingleOrDefaultAsync(broadcast => broadcast.Id == id, cancellationToken);

        return broadcast is not null ? broadcast : BroadcastErrors.BroadcastNotFound(id);
    }

    public async Task<Broadcast[]> GetAllUntrackedAsync<TKey>(
        Expression<Func<Broadcast, TKey>> sortKey,
        CancellationToken cancellationToken = default
    ) => await dbContext.Broadcasts.AsNoTracking().OrderBy(sortKey).ToArrayAsync(cancellationToken);

    public IQueryable<Broadcast> GetUntrackedQueryable() => dbContext.Broadcasts.AsNoTracking();

    public void Add(Broadcast aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Broadcasts.Add(aggregate);
    }

    public async Task<Result<Broadcast, IDomainError>> GetTrackedAsync(
        BroadcastId id,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id);

        Broadcast? broadcast = await dbContext.Broadcasts.SingleOrDefaultAsync(
            broadcast => broadcast.Id == id,
            cancellationToken
        );

        return broadcast is not null ? broadcast : BroadcastErrors.BroadcastNotFound(id);
    }

    public void Remove(Broadcast aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Broadcasts.Remove(aggregate);
    }

    public void Update(Broadcast aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Broadcasts.Update(aggregate);
    }
}
