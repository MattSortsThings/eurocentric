using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdProviders;

internal sealed class BroadcastIdProvider(TimeProvider timeProvider) : IBroadcastIdProvider
{
    public BroadcastId CreateSingle() => BroadcastId.FromValue(Guid.CreateVersion7(timeProvider.GetUtcNow()));
}
