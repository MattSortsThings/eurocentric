using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Generates <see cref="BroadcastId" /> value objects.
/// </summary>
public interface IBroadcastIdProvider
{
    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastId" /> instance with a unique <see cref="BroadcastId.Value" />.
    /// </summary>
    /// <returns>A new <see cref="BroadcastId" /> instance.</returns>
    public BroadcastId Create();
}
