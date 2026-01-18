using Eurocentric.Components.DataAccess.EFCore;
using Eurocentric.Domain.Persistence;

namespace Eurocentric.Components.Repositories;

internal sealed class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);
}
