using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdGenerators;

/// <summary>
///     Generates a <see cref="BroadcastId" /> instance on demand.
/// </summary>
internal sealed class BroadcastIdGenerator(TimeProvider timeProvider) : IBroadcastIdGenerator
{
    /// <inheritdoc />
    public BroadcastId Generate() => BroadcastId.Create(timeProvider.GetUtcNow());
}
