using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Core;
using SlimMessageBus;

namespace Eurocentric.Components.Repositories;

internal abstract class BaseWriteRepository
{
    private readonly IPublishBus _publishBus;

    protected BaseWriteRepository(IPublishBus publishBus, AppDbContext dbContext)
    {
        DbContext = dbContext;
        _publishBus = publishBus;
    }

    private protected AppDbContext DbContext { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IDomainEvent[] domainEvents = DbContext
            .ChangeTracker.Entries<IDomainEventSource>()
            .SelectMany(entry => entry.Entity.DetachAllDomainEvents())
            .ToArray();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await _publishBus.Publish(domainEvent, cancellationToken: cancellationToken);
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
