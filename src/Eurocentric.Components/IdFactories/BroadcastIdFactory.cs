using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Components.IdFactories;

internal sealed class BroadcastIdFactory(TimeProvider timeProvider) : IBroadcastIdFactory
{
    public BroadcastId Create() => BroadcastId.FromValue(Guid.CreateVersion7(timeProvider.GetUtcNow()));
}
