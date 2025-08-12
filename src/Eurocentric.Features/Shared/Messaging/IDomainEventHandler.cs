using Eurocentric.Domain.Abstractions;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     A handler for a domain event.
/// </summary>
/// <remarks>
///     Domain events are published to the message bus when immediately before a create/update/delete database
///     transaction is committed. Any domain event handler interface that uses the <see cref="AppDbContext" /> dependency
///     to retrieve and update domain entities must <i>NOT</i> invoke the <see cref="DbContext.SaveChanges()" /> or
///     <see cref="DbContext.SaveChangesAsync(CancellationToken)" /> or
///     <see cref="DbContext.SaveChangesAsync(bool, CancellationToken)" /> methods of the database context instance, as
///     changes are already being saved when the domain event handler is invoked.
/// </remarks>
/// <typeparam name="TDomainEvent"></typeparam>
internal interface IDomainEventHandler<in TDomainEvent> : IConsumer<TDomainEvent> where TDomainEvent : IDomainEvent;
