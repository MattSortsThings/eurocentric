using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Creates a <see cref="BroadcastId" /> instance on demand.
/// </summary>
public interface IBroadcastIdProvider
{
    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastId" /> instance with a <see cref="BroadcastId.Value" /> generated
    ///     according to RFC 9562, following the Version 7 format
    /// </summary>
    /// <returns>A new <see cref="BroadcastId" /> object.</returns>
    public BroadcastId CreateSingle();
}
