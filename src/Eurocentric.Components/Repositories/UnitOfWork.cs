using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Core;
using SlimMessageBus;

namespace Eurocentric.Components.Repositories;

internal sealed class UnitOfWork(AppDbContext dbContext, IPublishBus publishBus) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IDomainEvent[] domainEvents = dbContext
            .ChangeTracker.Entries<IDomainEventSource>()
            .SelectMany(entry => entry.Entity.DetachAllDomainEvents())
            .ToArray();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publishBus.Publish(domainEvent, cancellationToken: cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
