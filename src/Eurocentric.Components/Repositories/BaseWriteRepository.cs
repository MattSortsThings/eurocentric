using Eurocentric.Components.DataAccess.EfCore;

namespace Eurocentric.Components.Repositories;

internal abstract class BaseWriteRepository
{
    protected BaseWriteRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private protected AppDbContext DbContext { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await DbContext.SaveChangesAsync(cancellationToken);
}
