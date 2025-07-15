using ErrorOr;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Factory methods to create common broadcast aggregate errors.
/// </summary>
public static class BroadcastErrors
{
    /// <summary>
    ///     Creates and returns an <see cref="Error" /> indicating that the requested broadcast aggregate was not found.
    /// </summary>
    /// <param name="broadcastId">The requested broadcast ID.</param>
    /// <returns>A new <see cref="Error" /> instance.</returns>
    public static Error BroadcastNotFound(BroadcastId broadcastId) => Error.NotFound("Broadcast not found",
        "No broadcast exists with the provided broadcast ID.",
        new Dictionary<string, object> { { "broadcastId", broadcastId.Value } });
}
