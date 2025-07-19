using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Generates <see cref="BroadcastId" /> value objects on request.
/// </summary>
public interface IBroadcastIdGenerator
{
    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastId" /> value object.
    /// </summary>
    /// <returns>A new <see cref="BroadcastId" /> instance.</returns>
    public BroadcastId CreateSingle();
}
