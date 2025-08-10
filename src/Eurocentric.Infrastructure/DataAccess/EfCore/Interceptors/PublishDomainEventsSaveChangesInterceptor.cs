using Eurocentric.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SlimMessageBus;

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Interceptors;

internal sealed class PublishDomainEventsSaveChangesInterceptor(IMessageBus bus) : SaveChangesInterceptor
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

        IDomainEvent[] domainEvents = dbContext.ChangeTracker.Entries<IDomainEventSource>()
            .SelectMany(entry => entry.Entity.DetachAllDomainEvents())
            .ToArray();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await bus.Publish(domainEvent);
        }
    }
}
