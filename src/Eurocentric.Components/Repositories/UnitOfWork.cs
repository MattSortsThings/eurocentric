using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Abstractions.Repositories;

namespace Eurocentric.Components.Repositories;

internal sealed class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
}
