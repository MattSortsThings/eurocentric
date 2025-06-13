using Eurocentric.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SlimMessageBus;

namespace Eurocentric.Infrastructure.EFCore;

public sealed class PublishDomainEventsInterceptor(IMessageBus bus) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        PublishDomainEventsAsync(eventData).ConfigureAwait(false).GetAwaiter().GetResult();

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        await PublishDomainEventsAsync(eventData);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(DbContextEventData eventData)
    {
        if (eventData.Context is not { } dbContext)
        {
            return;
        }

        Entity[] entities = dbContext.ChangeTracker.Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents.Count != 0)
            .Select(entry => entry.Entity)
            .ToArray();

        List<IDomainEvent> domainEvents = entities.SelectMany(entity => entity.DomainEvents).ToList();

        foreach (Entity entity in entities)
        {
            entity.ClearDomainEvents();
        }

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await bus.Publish(domainEvent);
        }
    }
}
