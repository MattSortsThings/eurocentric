using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories;

internal sealed class BroadcastReadRepository(AppDbContext dbContext) : IBroadcastReadRepository
{
    public async Task<Broadcast[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Broadcasts.AsNoTracking()
            .OrderBy(contest => contest.BroadcastDate)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Result<Broadcast, IDomainError>> GetByIdAsync(
        BroadcastId broadcastId,
        CancellationToken cancellationToken = default
    )
    {
        Broadcast? broadcast = await dbContext
            .Broadcasts.AsNoTracking()
            .SingleOrDefaultAsync(broadcast => broadcast.Id.Equals(broadcastId), cancellationToken);

        return broadcast is not null ? broadcast : BroadcastErrors.BroadcastNotFound(broadcastId);
    }

    public IQueryable<Broadcast> GetAsQueryable() => dbContext.Broadcasts.AsNoTracking();
}
