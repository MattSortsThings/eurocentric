using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Domain errors that may occur when working with <see cref="Broadcast" /> aggregates.
/// </summary>
public static class BroadcastErrors
{
    /// <summary>
    ///     Creates and returns a new error indicating that the client has requested a <see cref="Broadcast" /> that
    ///     does not exist in the system.
    /// </summary>
    /// <param name="broadcastId">The ID of the requested country.</param>
    /// <returns>A new <see cref="NotFoundError" /> instance.</returns>
    public static NotFoundError BroadcastNotFound(BroadcastId broadcastId)
    {
        return new NotFoundError
        {
            Title = "Broadcast not found",
            Detail = "The requested broadcast does not exist.",
            Extensions = new Dictionary<string, object?> { { nameof(broadcastId), broadcastId.Value } },
        };
    }
}
