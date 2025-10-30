using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

public interface IBroadcastIdFactory
{
    /// <summary>
    ///     Creates and returns a new <see cref="BroadcastId" /> instance.
    /// </summary>
    /// <returns>A new <see cref="BroadcastId" /> instance.</returns>
    BroadcastId Create();
}
