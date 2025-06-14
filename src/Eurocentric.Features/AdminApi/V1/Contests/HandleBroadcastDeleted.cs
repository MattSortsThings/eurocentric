using Eurocentric.Domain.Events;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V1.Contests;

internal static class HandleBroadcastDeleted
{
    internal sealed class Handler : IDomainEventHandler<BroadcastDeletedEvent>
    {
        public Task OnHandle(BroadcastDeletedEvent message, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
