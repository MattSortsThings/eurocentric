using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdGenerators;

/// <summary>
///     Generates <see cref="BroadcastId" /> value objects.
/// </summary>
/// <param name="timeProvider">Provides the system time.</param>
internal sealed class BroadcastIdGenerator(TimeProvider timeProvider) : IBroadcastIdGenerator
{
    /// <inheritdoc />
    public BroadcastId CreateSingle() => BroadcastId.Create(timeProvider.GetUtcNow());
}
