using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Broadcasts;
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
}
